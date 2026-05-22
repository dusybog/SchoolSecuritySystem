// FormParser.js
import { formSchema } from './FormSchema.js';

export function extractFormData() {
    const formEl = document.getElementById('incident-main-form');
    if (!formEl) return null;

    if (!formEl.checkValidity()) {
        formEl.classList.add('was-validated');
        alert('⚠️ 儲存失敗！請檢查並填寫所有標示紅框的必填欄位。');
        return null;
    }

    const result = {};

    // 1. 萃取 Static 區塊資料
    ['basic', 'details'].forEach(key => {
        result[key] = {};
        formSchema[key].fields.forEach(f => {
            if (f.type === 'slot') return; // 略過插槽欄位

            const col = formEl.querySelector(`[name="${key}_${f.name}"]`)?.closest('.col-md-4, .col-md-12');
            if (col && col.classList.contains('d-none')) return;

            if (f.type === 'checkbox') {
                const checked = Array.from(formEl.querySelectorAll(`input[name="${key}_${f.name}"]:checked`)).map(cb => cb.value);
                result[key][f.name] = checked;
            } else {
                const input = formEl.querySelector(`[name="${key}_${f.name}"]`);
                result[key][f.name] = input ? input.value : '';
            }
        });

        // 自動為 basic 區塊組裝 title 欄位
        if (key === 'basic' && result.basic.mainCategory && result.basic.eventName) {
            const eventSelect = formEl.querySelector(`select[name="basic_eventName"]`);

            if (eventSelect && eventSelect.selectedIndex > 0) {
                const selectedOption = eventSelect.options[eventSelect.selectedIndex];
                const optgroup = selectedOption.closest('optgroup');
                const groupLabel = optgroup ? optgroup.label.replace('📂 ', '') : '';
                result.basic.title = `${result.basic.mainCategory} - ${groupLabel}`;
            }
        }
    });

    // 2. 萃取 Dynamic 區塊資料
    ['persons', 'properties', 'attachments'].forEach(key => {
        result[key] = [];
        const rows = formEl.querySelectorAll(`.${key}-row`);
        rows.forEach(row => {
            const item = {};
            let hasValue = false;

            formSchema[key].fields.forEach(f => {
                const input = row.querySelector(`[data-field="${f.name}"]`);
                if (input) {
                    item[f.name] = input.value;
                    if (input.value.trim() !== '') hasValue = true;
                }
            });

            if (hasValue) result[key].push(item);
        });
    });

    // 3. 萃取 Slot 內動態產生的客製化問題
    result.conditionalData = {};
    const dynamicInputs = formEl.querySelectorAll('.dynamic-input');

    dynamicInputs.forEach(input => {
        if (input.disabled) return;

        if (input.type === 'radio' || input.type === 'checkbox') {
            if (input.checked) {
                if (!result.conditionalData[input.name]) {
                    result.conditionalData[input.name] = input.type === 'radio' ? input.value : [input.value];
                } else if (input.type === 'checkbox') {
                    result.conditionalData[input.name].push(input.value);
                }
            }
        } else {
            if (input.value.trim() !== '') {
                result.conditionalData[input.name] = input.value;
            }
        }
    });

    // ==========================================
    // 🌟 關鍵修正：重組回傳格式以符合 CreateSubmissionDto
    // ==========================================

    // 確保 basic 存在，避免出現 Cannot read properties of undefined 的錯誤
    const basicData = result.basic || {};

    // 依照後端 DTO 定義，組裝最終的 Payload
    const finalPayload = {
        // --- 第一部分：扁平化的基本必填欄位 ---
        reporter: basicData.reporter || '',
        phone: basicData.phone || '',
        // 將 department 轉為數字 (long)，若無法轉換則帶 0 (會被後端的 Required 攔截)
        department: parseInt(basicData.department, 10) || 0,
        mainCategory: basicData.mainCategory || '',
        eventName: basicData.eventName || '',
        title: basicData.title || '',

        // --- 第二部分：JsonElement 區塊，保持為物件結構，讓後端自動轉型為 JSON ---
        basic: result.basic || {},
        details: result.details || {},
        persons: result.persons || [],
        properties: result.properties || [],
        attachments: result.attachments || [],
        conditionalData: result.conditionalData || {}
    };

    return finalPayload;
}