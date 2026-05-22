// FormSchema.js

let departmentOptions = [];

try {
    const response = await fetch('/api/departments/options');
    if (response.ok) {
        departmentOptions = await response.json();
    } else {
        console.warn('無法取得系所選單，狀態碼:', response.status);
    }
} catch (error) {
    console.error('載入系所選單發生網路錯誤:', error);
}

export const formSchema = {
    basic: {
        title: '基本資料',
        type: 'static',
        fields: [
            { name: 'reporter', label: '通報人員', type: 'text' },
            { name: 'phone', label: '連絡電話', type: 'text' },
            { name: 'department', label: '通報系所', type: 'select', options: departmentOptions },
            { name: 'mainCategory', label: '主類別', type: 'select', options: [] },
            { name: 'eventName', label: '事件名稱', type: 'select', options: [] },

            // 用來動態插入 EventCategoryHandler 產生的進階問題
            { name: 'conditionalQuestionsSlot', type: 'slot' },
        ]
    },
    details: {
        title: '事件詳細內容',
        type: 'static',
        fields: [
            { name: 'incidentTime', label: '發生時間', type: 'datetime-local' },
            { name: 'knownTime', label: '學校知悉時間', type: 'datetime-local' },
            { name: 'incidentLocation', label: '發生地點', type: 'select', options: ['校內一般場所', '校內實驗/實習場所', '校外場所（國內）', '校外場所（國外）'] },
            { name: 'source', label: '消息來源', type: 'select', options: ['警察', '教職員工', '家長', '學生', '民眾', '教育局', '校外會', '媒體', '其他'] },
            { name: 'mediaAware', label: '媒體是否得知', type: 'select', options: ['否', '是'] },
            {
                name: 'newsUrl', label: '請貼上新聞網址', type: 'text',
                showIf: { field: 'mediaAware', contains: '是' }
            },
            { name: 'involveOther', label: '是否涉及他校', type: 'select', options: ['否', '是'] },
            {
                name: 'otherSchoolName', label: '請輸入涉及學校之名稱', type: 'text',
                showIf: { field: 'involveOther', contains: '是' }
            }, ,
            { name: 'summary', label: '事件摘要', type: 'textarea', placeholder:'請簡要描述事件的人、事、時、地、物...'},
            { name: 'causeAndProcess', label: '事件原因及經過', type: 'textarea', placeholder: '請詳述事件發生的原因與詳細經過...'},
            { name: 'handlingStatus', label: '處理情形', type: 'checkbox', options: ['聯繫家屬', '送醫治療', '探望學生', '瞭解傷情', '其他'] },
            {
                name: 'otherStatusDetail', label: '請說明其他處理情形', type: 'text',
                showIf: { field: 'handlingStatus', contains: '其他' }
            },
            { name: 'improvement', label: '具體檢討及改進措施', type: 'textarea', placeholder: '請條列說明後續的檢討與改進計畫...'}
        ]
    },
    persons: {
        title: '主要人物資料 (可新增多筆)',
        type: 'dynamic',
        fields: [
            { name: 'name', label: '姓名', type: 'text' },
            { name: 'departmentOrClass', label: '系級/處室', type: 'text' },
            { name: 'id', label: '學號/人員代號', type: 'text' },
            { name: 'birthYear', label: '出生年', type: 'number', min: 1900, max: new Date().getFullYear() },
            { name: 'gender', label: '性別', type: 'select', options: ['男', '女'] },
            { name: 'status', label: '狀態', type: 'select', options: ['正常', '輕傷', '重傷', '死亡', '失蹤', '疾病'] },
            { name: 'jobTitle', label: '職稱', type: 'select', options: ['學生', '教師', '職員', '家長', '校外人士', '校長(園長)', '其他'] },
            { name: 'location', label: '目前位置', type: 'select', options: ['學校', '家中', '醫院', '警局', '安置中', '不明', '其他'] },
            { name: 'role', label: '角色', type: 'select', options: ['受害人', '肇事人', '關係人', '其他'] },
            { name: 'hasSimilarIncident', label: '是否曾發生類似事件', type: 'select', options: ['否', '是'] },
            { name: 'isSchoolMember', label: '是否為本校教職員生', type: 'select', options: ['是', '否'] },
            { name: 'note', label: '備註', type: 'text', required: false }
        ]
    },
    properties: {
        title: '財損資料 (可新增多筆)',
        type: 'dynamic',
        fields: [
            { name: 'itemName', label: '品名', type: 'text' },
            { name: 'attribute', label: '屬性', type: 'select', options: ['車輛', '建物', '教學設備', '行政/辦公設備', '其他公共財務', '其他非公共財務'] },
            { name: 'status', label: '狀態', type: 'select', options: ['半毀', '全毀', '部分遺失', '全部遺失'] },
            { name: 'quantity', label: '數量', type: 'number', min: 1 },
            { name: 'unit', label: '單位', type: 'select', options: ['輛', '棟', '間', '臺', '座', '個', '組', '其他'] },
            { name: 'amount', label: '金額 (元)', type: 'number', min: 0 },
            { name: 'hasInsurance', label: '有無保險', type: 'select', options: ['無', '有', '不確定'] }
        ]
    },
    attachments: {
        title: '附件 (可新增多筆)',
        type: 'dynamic',
        fields: [
            { name: 'description', label: '描述', type: 'text' },
            { name: 'fileUrl', label: '檔案URL', type: 'text' }
        ]
    }
};