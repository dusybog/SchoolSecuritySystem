export const formatSubmissionStatus = (val) => {
    const statusMap = {
        0: { text: '新建立', color: '#4b5563', bg: '#f3f4f6' },
        10: { text: '系所已審核', color: '#c2410c', bg: '#ffedd5' },
        20: { text: '中心已審核', color: '#1d4ed8', bg: '#dbeafe' },
        30: { text: '已結案', color: '#047857', bg: '#d1fae5' }
    };

    const target = statusMap[val];
    if (!target) return val;

    return `<span style="padding: 4px 8px; border-radius: 4px; font-size: 13px; font-weight: bold; color: ${target.color}; background-color: ${target.bg};">
                ${target.text}
            </span>`;
};

export const formatDateTime = (val) => {
    if (!val || val === "-") return val;
    const d = new Date(val);
    const yyyy = d.getFullYear();
    const MM = String(d.getMonth() + 1).padStart(2, '0');
    const dd = String(d.getDate()).padStart(2, '0');
    const HH = String(d.getHours()).padStart(2, '0');
    const mm = String(d.getMinutes()).padStart(2, '0');
    return `${yyyy}-${MM}-${dd} ${HH}:${mm}`;
};