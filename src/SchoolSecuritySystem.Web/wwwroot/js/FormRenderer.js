// FormRenderer.js
import { formSchema } from './FormSchema.js';

export function renderForm(containerId, initialData = {}) {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';

    const formEl = document.createElement('form');
    formEl.id = 'incident-main-form';
    formEl.className = 'needs-validation';
    formEl.onsubmit = (e) => e.preventDefault();

    for (const [sectionKey, sectionDef] of Object.entries(formSchema)) {
        const sectionCard = createSectionCard(sectionKey, sectionDef, initialData);
        if (sectionCard) formEl.appendChild(sectionCard);
    }
    container.appendChild(formEl);

    bindShowIfLogic(formEl);
}

export function renderReadOnly(containerId, jsonData) {
    renderForm(containerId, jsonData);

    const container = document.getElementById(containerId);
    const inputs = container.querySelectorAll('input, select, textarea, button');
    inputs.forEach(el => {
        if (el.tagName === 'BUTTON') {
            el.style.display = 'none';
        } else {
            el.disabled = true;
            el.classList.add('bg-light');
            el.removeAttribute('required');
        }
    });
}

function createSectionCard(sectionKey, sectionDef, formData) {
    // 支援全域插槽
    if (sectionDef.type === 'slot') {
        const slotDiv = document.createElement('div');
        slotDiv.id = `slot-${sectionKey}`;
        slotDiv.className = 'custom-slot-container mb-4';
        return slotDiv;
    }

    // 主題色彩配對表
    const colorMap = {
        basic: { bg: 'bg-primary', text: 'text-white' },
        details: { bg: 'bg-success', text: 'text-white' },
        persons: { bg: 'bg-warning', text: 'text-dark' },
        properties: { bg: 'bg-danger', text: 'text-white' },
        attachments: { bg: 'bg-info', text: 'text-dark' }
    };

    const theme = colorMap[sectionKey] || { bg: 'bg-secondary', text: 'text-white' };

    const card = document.createElement('div');
    card.className = `card shadow mb-4 border-0`;
    let headerHtml = `<div class="card-header ${theme.bg} ${theme.text} fw-bold fs-5">${sectionDef.title}</div>`;
    let bodyHtml = `<div class="card-body bg-white p-4" id="section-${sectionKey}"></div>`;

    card.innerHTML = headerHtml + bodyHtml;

    const bodyEl = card.querySelector(`#section-${sectionKey}`);

    const renderInnerContent = (dataToRender, showWarning = false) => {
        bodyEl.innerHTML = '';

        if (showWarning) {
            const warningAlert = document.createElement('div');
            warningAlert.className = 'alert alert-warning border-warning shadow-sm mb-4';
            warningAlert.innerHTML = `
                <div class="d-flex align-items-center">
                    <i class="bi bi-exclamation-triangle-fill text-warning fs-4 me-3"></i>
                    <div>
                        <strong class="text-dark">⚠️ 資料格式有誤，請與填表者聯繫以釐清內容。</strong>
                        <div class="small text-muted mt-1">系統已啟動保護機制，自動將此區塊降級為空白表單。</div>
                    </div>
                </div>`;
            bodyEl.appendChild(warningAlert);
        }

        if (sectionDef.type === 'static') {
            const row = document.createElement('div');
            row.className = 'row g-3';
            sectionDef.fields.forEach(f => {
                const val = dataToRender[f.name] !== undefined ? dataToRender[f.name] : '';
                row.appendChild(createInputCol(f, `${sectionKey}_${f.name}`, val));
            });
            bodyEl.appendChild(row);
        }
        else if (sectionDef.type === 'dynamic') {
            const wrapper = document.createElement('div');
            wrapper.id = `wrapper-${sectionKey}`;
            bodyEl.appendChild(wrapper);

            const items = Array.isArray(dataToRender) ? dataToRender : [];
            items.forEach(itemData => wrapper.appendChild(createDynamicRow(sectionKey, sectionDef.fields, itemData)));

            const addBtn = document.createElement('button');
            addBtn.type = 'button';
            addBtn.className = 'btn btn-outline-primary btn-sm mt-3 fw-bold';
            addBtn.innerHTML = `➕ 新增一筆`;
            addBtn.onclick = () => wrapper.appendChild(createDynamicRow(sectionKey, sectionDef.fields, {}));
            bodyEl.appendChild(addBtn);
        }
    };

    try {
        let safeData = formData[sectionKey];
        let isFormatWrong = false;

        if (safeData !== undefined && safeData !== null) {
            if (sectionDef.type === 'static' && (typeof safeData !== 'object' || Array.isArray(safeData))) {
                isFormatWrong = true;
            }
            if (sectionDef.type === 'dynamic' && !Array.isArray(safeData)) {
                isFormatWrong = true;
            }
        }

        if (isFormatWrong) {
            console.warn(`[資料防護] 區塊 ${sectionKey} 資料結構異常，已攔截。`);
            renderInnerContent(sectionDef.type === 'dynamic' ? [] : {}, true);
        } else {
            safeData = safeData || (sectionDef.type === 'dynamic' ? [] : {});
            renderInnerContent(safeData, false);
        }
    } catch (error) {
        console.error(`[系統防護] 渲染區塊 ${sectionKey} 時發生嚴重錯誤，退回空白表單:`, error);
        renderInnerContent(sectionDef.type === 'dynamic' ? [] : {}, true);
    }

    // 🌟 新增：鎖定基本資料區塊 (或 Schema 中指定 readonly 的區塊)
    //if (sectionKey === 'basic' || sectionDef.readonly) {
    //    const inputs = card.querySelectorAll('input, select, textarea, button');
    //    inputs.forEach(el => {
    //        if (el.tagName === 'BUTTON') {
    //            el.style.display = 'none'; // 隱藏按鈕 (如新增一筆、刪除)
    //        } else {
    //            el.disabled = true; // 鎖定輸入框
    //            el.classList.add('bg-light'); // 加上背景色提示不可編輯
    //            el.removeAttribute('required'); // 拔除必填驗證，避免卡住表單送出
    //        }
    //    });
    //}

    return card;
}

function createDynamicRow(sectionKey, fields, itemData) {
    const row = document.createElement('div');
    row.className = `row g-3 align-items-end mb-3 pb-3 border-bottom ${sectionKey}-row position-relative`;

    fields.forEach(f => {
        const val = itemData[f.name] || '';
        const col = createInputCol(f, '', val);
        const input = col.querySelector('input, select, textarea');
        if (input) {
            input.removeAttribute('name');
            input.setAttribute('data-field', f.name);
        }
        row.appendChild(col);
    });

    const delCol = document.createElement('div');
    delCol.className = 'col-md-12 text-end';
    delCol.innerHTML = `<button type="button" class="btn btn-sm btn-outline-danger shadow-sm">🗑️ 移除此筆</button>`;
    delCol.querySelector('button').onclick = () => row.remove();
    row.appendChild(delCol);

    return row;
}

function createInputCol(field, nameAttr, value) {
    // 🌟 支援區域插槽 (欄位層級)
    if (field.type === 'slot') {
        const slotCol = document.createElement('div');
        slotCol.className = 'col-md-12';
        slotCol.id = `slot-${field.name}`;
        return slotCol;
    }

    const col = document.createElement('div');
    const isFullWidth = field.type === 'textarea' || field.type === 'checkbox';
    col.className = isFullWidth ? 'col-md-12' : 'col-md-4';

    const isRequired = field.required !== false ? 'required' : '';
    const reqStar = isRequired ? '<span class="text-danger ms-1">*</span>' : '';

    let inputHtml = '';

    if (field.type === 'select') {
        // 🌟 支援字串 ['A', 'B'] 或物件 [{id:1, name:'A'}, ...] 格式
        const options = field.options.map(o => {
            const optVal = typeof o === 'object' ? (o.id || o.value) : o;
            const optText = typeof o === 'object' ? (o.name || o.text) : o;
            // 注意：要轉成字串比對，避免數字 1 跟字串 "1" 對不起來
            const isSelected = String(value) === String(optVal) ? 'selected' : '';
            return `<option value="${optVal}" ${isSelected}>${optText}</option>`;
        }).join('');

        inputHtml = `<select class="form-select" name="${nameAttr}" ${isRequired}>
                        <option value="">請選擇...</option>
                        ${options}
                     </select>`;
    }
    else if (field.type === 'textarea') {
        const placeholderAttr = field.placeholder ? `placeholder="${field.placeholder}"` : '';
        inputHtml = `<textarea class="form-control" name="${nameAttr}" rows="3" ${placeholderAttr} ${isRequired}>${value}</textarea>`;
    }
    else if (field.type === 'checkbox') {
        const valArray = Array.isArray(value) ? value : [];
        const checks = field.options.map((o, idx) => `
            <div class="form-check form-check-inline">
                <input class="form-check-input" type="checkbox" name="${nameAttr}" value="${o}" id="${nameAttr}_${idx}" ${valArray.includes(o) ? 'checked' : ''}>
                <label class="form-check-label" for="${nameAttr}_${idx}">${o}</label>
            </div>
        `).join('');
        inputHtml = `<div>${checks}</div>`;
    }
    else {
        const minAttr = field.min !== undefined ? `min="${field.min}"` : '';
        const maxAttr = field.max !== undefined ? `max="${field.max}"` : '';
        const placeholderAttr = field.placeholder ? `placeholder="${field.placeholder}"` : '';
        inputHtml = `<input type="${field.type}" class="form-control" name="${nameAttr}" value="${value}" ${minAttr} ${maxAttr} ${placeholderAttr} ${isRequired}>`;
    }

    col.innerHTML = `<label class="form-label text-secondary small fw-bold mb-1">${field.label}${reqStar}</label>
                     ${inputHtml}`;

    if (field.showIf) {
        col.classList.add('d-none', 'conditional-field');
        col.setAttribute('data-cond-target', field.showIf.field);
        col.setAttribute('data-cond-val', field.showIf.contains || field.showIf.equals);

        // 安全防呆：確認有找到 input 元素再移除 required
        if (isRequired) {
            const inputEl = col.querySelector('input, select, textarea');
            if (inputEl) inputEl.removeAttribute('required');
        }
    }

    return col;
}

function bindShowIfLogic(formEl) {
    const checkConditions = () => {
        const conditionCols = formEl.querySelectorAll('.conditional-field');
        conditionCols.forEach(col => {
            const targetName = col.getAttribute('data-cond-target');
            const targetVal = col.getAttribute('data-cond-val');

            const targetInputs = formEl.querySelectorAll(`input[name="details_${targetName}"]:checked, select[name="details_${targetName}"]`);

            let isMatch = false;
            targetInputs.forEach(input => {
                if (input.value === targetVal || (input.tagName === 'SELECT' && input.value === targetVal)) {
                    isMatch = true;
                }
            });

            const myInput = col.querySelector('input, select, textarea');
            if (isMatch) {
                col.classList.remove('d-none');
                myInput.setAttribute('required', 'true');
            } else {
                col.classList.add('d-none');
                myInput.removeAttribute('required');
                myInput.value = '';
            }
        });
    };

    formEl.addEventListener('change', checkConditions);
    checkConditions();
}