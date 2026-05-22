// EventCategoryHandler.js
import { eventData } from './eventData.js';

/**
 * 綁定主類別與事件名稱的連動，並處理 Slot 渲染。
 * @param {object} initialDetailsData 為了支援 Edit 模式，可傳入 details 的回填資料
 */
export function bindEventCategoryLogic(initialBasicData = null) {
    const mainCategorySelect = document.querySelector('select[name="basic_mainCategory"]');
    const eventNameSelect = document.querySelector('select[name="basic_eventName"]');
    const conditionalSlot = document.getElementById('slot-conditionalQuestionsSlot');

    if (!mainCategorySelect || !eventNameSelect || !conditionalSlot) return;

    // 1. 初始化「主類別」選項
    if (mainCategorySelect.options.length <= 1) {
        mainCategorySelect.innerHTML = '<option value="">請選擇主類別...</option>';
        Object.keys(eventData).forEach(category => {
            mainCategorySelect.innerHTML += `<option value="${category}">${category}</option>`;
        });
    }

    // 2. 當「主類別」改變時 -> 更新「事件名稱」選單
    mainCategorySelect.addEventListener('change', (e) => {
        const category = e.target.value;
        eventNameSelect.innerHTML = '<option value="">請選擇事件名稱...</option>';
        conditionalSlot.innerHTML = '';

        if (category && eventData[category]) {
            const subCategories = [...new Set(eventData[category].map(item => item.subCategory))];

            subCategories.forEach(sub => {
                const groupEvents = eventData[category].filter(item => item.subCategory === sub);
                let optgroup = `<optgroup label="📂 ${sub}">`;
                groupEvents.forEach(evt => {
                    optgroup += `<option value="${evt.eventName}">${evt.eventName}</option>`;
                });
                optgroup += `</optgroup>`;
                eventNameSelect.innerHTML += optgroup;
            });
        }
    });

    // 3. 當「事件名稱」改變時 -> 渲染進階問題到 Slot
    eventNameSelect.addEventListener('change', (e) => {
        const category = mainCategorySelect.value;
        const eventName = e.target.value;
        conditionalSlot.innerHTML = '';

        if (!category || !eventName) return;

        const targetEvent = eventData[category].find(item => item.eventName === eventName);

        if (targetEvent && targetEvent.conditionalFields) {
            let html = `<div class="col-12 mt-3 mb-2 p-3 bg-warning bg-opacity-10 border border-warning rounded-3 shadow-sm">
                            <h6 class="text-dark fw-bold mb-3">⚠️ 此事件需填寫進階資訊</h6>
                            <div class="row g-3">`;

            targetEvent.conditionalFields.forEach(field => {
                html += renderConditionalField(field);
            });

            html += `</div></div>`;
            conditionalSlot.innerHTML = html;
        }
    });

    // 4. 處理「其他 (需填寫文字)」的輸入框解鎖
    conditionalSlot.addEventListener('change', (e) => {
        if (e.target.classList.contains('has-text-trigger')) {
            const textInputId = e.target.dataset.targetInput;
            const textInput = document.getElementById(textInputId);
            if (textInput) {
                textInput.disabled = !e.target.checked;
                textInput.required = e.target.checked;
                if (!e.target.checked) textInput.value = '';
            }
        }
    });

    // ==========================================
    // 🌟 5. 處理 Edit 模式的自動回填 (Auto-Fill)
    // ==========================================
    if (initialBasicData && initialBasicData.mainCategory) {
        mainCategorySelect.value = initialBasicData.mainCategory;
        mainCategorySelect.dispatchEvent(new Event('change'));

        eventNameSelect.value = initialBasicData.eventName;
        eventNameSelect.dispatchEvent(new Event('change'));
    }
}

// 內部 HTML 產生器
// --- 內部函數：負責把 JSON 定義的進階欄位轉成 Bootstrap 5 HTML ---
function renderConditionalField(field) {
    let inputHtml = '';

    // 🌟 1. 支援 placeholder 屬性
    const placeholderAttr = field.placeholder ? `placeholder="${field.placeholder}"` : '';

    // 🌟 2. 支援 required 屬性 (預設必填，除非在 JSON 明確寫 required: false)
    const isRequired = field.required !== false ? 'required' : '';

    if (field.type === 'radio' || field.type === 'checkbox') {
        field.options.forEach((opt, idx) => {
            const isObj = typeof opt === 'object';
            const label = isObj ? opt.label : opt;
            const val = label;
            const id = `${field.id}_${idx}`;

            inputHtml += `<div class="form-check mb-2">
                <input class="form-check-input dynamic-input ${isObj && opt.hasTextInput ? 'has-text-trigger' : ''}" 
                       type="${field.type}" 
                       name="${field.id}" 
                       value="${val}" 
                       id="${id}" 
                       ${isObj && opt.hasTextInput ? `data-target-input="text_${id}"` : ''} 
                       ${isRequired}>
                <label class="form-check-label" for="${id}">${label}</label>`;

            if (isObj && opt.hasTextInput) {
                inputHtml += `
                    <input type="text" id="text_${id}" name="${field.id}_otherText" 
                           class="form-control form-control-sm mt-1 dynamic-input" 
                           placeholder="${opt.placeholder || '請說明...'}" disabled>
                `;
            }
            inputHtml += `</div>`;
        });

        if (field.type === 'checkbox') inputHtml = inputHtml.replace(/required/g, '');

    } else if (field.type === 'text') {
        // 🌟 支援 Text 類型的 placeholder
        inputHtml = `<input type="text" name="${field.id}" class="form-control dynamic-input" ${placeholderAttr} ${isRequired}>`;
    } else if (field.type === 'datetime') {
        // 🌟 3. 支援 Datetime 類型 (HTML5 原生 datetime-local)
        inputHtml = `<input type="datetime-local" name="${field.id}" class="form-control dynamic-input" ${isRequired}>`;
    } else if (field.type === 'date') {
        // 🌟 同場加映：順便支援純日期類型
        inputHtml = `<input type="date" name="${field.id}" class="form-control dynamic-input" ${isRequired}>`;
    }

    // 🌟 4. 支援 description (說明文字)
    const descriptionHtml = field.description
        ? `<div class="form-text text-muted small mt-1"><i class="bi bi-info-circle"></i> ${field.description}</div>`
        : '';

    return `<div class="col-md-12">
                <label class="form-label text-primary fw-bold mb-2">${field.label}</label>
                <div class="p-2 border rounded bg-white">
                    ${inputHtml}
                    ${descriptionHtml}
                </div>
            </div>`;
}