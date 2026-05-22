export const eventData = {
    "意外事件": [
        { subCategory: "交通意外事件", eventName: "校內交通意外事件", reportType: 'general'},
        { subCategory: "交通意外事件", eventName: "校外教學交通意外事件", reportType: 'statutory', },
        { subCategory: "交通意外事件", eventName: "校外交通意外事件", reportType: 'general' },
        { subCategory: "中毒事件", eventName: "食品中毒",reportType: 'general' },
        { subCategory: "中毒事件", eventName: "實驗室毒性化學物質中毒", reportType: 'general' },
        { subCategory: "中毒事件", eventName: "其他化學品中毒", reportType: 'general' },
        { subCategory: "自傷、自殺事件", eventName: "學生自殺、自傷", reportType: 'general',
            conditionalFields: [
                {
                    id: 'suicide_type', // 欄位的唯一ID，會當作JSON的key
                    label: '學生自殺、自傷事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)
                        "自殺死亡(有企圖結束生命的行為，但未死亡)",
                        "自殺企圖(有企圖結束生命的行為，但未死亡)",
                        "自殺意念(有企圖結束生命的想法，但未有行動)",
                        "自傷行為(有自我傷害的行為，但非意圖結束死亡)",
                        "自傷意念(有自我傷害的想法，但非意圖結束死亡)"
                    ]
                }
            ]
        },
        { subCategory: "自傷、自殺事件", eventName: "教職員工自殺、自傷", reportType: 'general' },
        { subCategory: "溺水事件", eventName: "溺水事件", reportType: 'general' },
        { subCategory: "運動、休閒事件", eventName: "運動、休閒事件", reportType: 'general' },
        { subCategory: "運動、休閒事件", eventName: "墜樓事件(非自殺)",reportType: 'general' },
        { subCategory: "運動、休閒事件", eventName: "山難事件", reportType: 'general' },
        { subCategory: "實驗、實習及環境設施事件", eventName: "實驗、實習傷害", reportType: 'general' },
        { subCategory: "實驗、實習及環境設施事件", eventName: "工地整建傷人事件", reportType: 'general' },
        { subCategory: "實驗、實習及環境設施事件", eventName: "建築物坍塌傷人事件", reportType: 'general' },
        { subCategory: "實驗、實習及環境設施事件", eventName: "工讀(建教)場所傷害", reportType: 'general' },
        { subCategory: "實驗、實習及環境設施事件", eventName: "因校內設施(備)、器材受傷", reportType: 'general' },
        { subCategory: "其他意外傷害事件", eventName: "其他意外傷害事件", reportType: 'general' }
    ],
    "安全維護事件": [
        { subCategory: "校園性別事件", eventName: "知悉疑似 18 歲以上性侵害事件(性別平等教育法或非屬性別平等教育法)", reportType: 'general',
            conditionalFields: [
                {
                    id: 'gender_type1', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ] },
        { subCategory: "校園性別事件", eventName: "知悉疑似 18 歲以上性騷擾事件(性別平等教育法或非屬性別平等教育法)", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'gender_type2-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                },
                {
                    id: 'gender_type2-2', // 欄位的唯一ID，會當作JSON的key
                    label: '數位/網路性別暴力事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]  },
        { subCategory: "校園性別事件", eventName: "知悉疑似 18 歲以上性霸凌事件", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'gender_type3', // 欄位的唯一ID，會當作JSON的key
                    label: '數位/網路性別暴力事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]  },
        { subCategory: "校園性別事件", eventName: "知悉疑似校長或教職員工違反與性或性別有關之專業倫理行為", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'gender_type4-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                },
                {
                    id: 'gender_type4-2', // 欄位的唯一ID，會當作JSON的key
                    label: '數位/網路性別暴力事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "家庭暴力事件", eventName: "知悉疑似家庭暴力情事", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'familybrutal_type1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                }
            ]   },
        { subCategory: "家庭暴力事件", eventName: "校園親密關係暴力事件（屬家庭暴力防治法第六十三條之一，學生間發生未同居親密關係暴力事件）", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'familybrutal_type2', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                }
            ]   },
        { subCategory: "身心障礙事件", eventName: "知悉遺棄身心障礙者", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉對身心障礙者身心虐待", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉對身心障礙者限制其自由", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉留置無生活自理能力之身心障礙者於易發生危險或傷害之環境", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉利用身心障礙者行乞或供人參觀", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉強迫或誘騙身心障礙者結婚", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉其他對身心障礙者或利用身心障礙者為犯罪或不正當之行為", reportType: 'statutory' },
        { subCategory: "身心障礙事件", eventName: "知悉家庭暴力情事者", reportType: 'statutory' },
        { subCategory: "火警", eventName: "校內火警", reportType: 'general' },
        { subCategory: "火警", eventName: "校外火警", reportType: 'general' },
        { subCategory: "人為破壞事件", eventName: "校內設施(備)遭破壞", reportType: 'general' },
        { subCategory: "人為破壞事件", eventName: "爆裂物危害", reportType: 'general' },
        { subCategory: "校園失竊事件", eventName: "校屬財產、器材遭竊", reportType: 'general' },
        { subCategory: "校園失竊事件", eventName: "其他財物遭竊", reportType: 'general' },
        { subCategory: "糾紛事件", eventName: "賃居糾紛事件", reportType: 'general' },
        { subCategory: "糾紛事件", eventName: "交易糾紛", reportType: 'general' },
        { subCategory: "糾紛事件", eventName: "網路糾紛", reportType: 'general' },
        { subCategory: "校屬人員遭侵害事件", eventName: "遭殺害", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type1', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校屬人員遭侵害事件", eventName: "遭強盜搶奪", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type2', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校屬人員遭侵害事件", eventName: "遭恐嚇勒索", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type3', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校屬人員遭侵害事件", eventName: "遭擄人勒贖", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type4', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校屬人員遭侵害事件", eventName: "其他遭暴力、侵害或強制行為", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type5', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校屬人員遭侵害事件", eventName: "師生遭騷擾、脅迫等事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'campusmember_type6-2', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                },
                {
                    id: 'campusmember_type6-1', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為家長對他生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "資訊安全", eventName: "遭外人入侵、破壞各級學校及幼兒園資訊系統", reportType: 'general' },
        { subCategory: " 詐騙事件", eventName: "詐騙事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'fraud_methods',
                    label: '詐騙手法 (可複選):',
                    type: 'checkbox',
                    // 【修改這裡】options 陣列現在混合了字串和物件
                    options: [
                        "解除分期付款詐騙(ATM)",
                        "假網路拍賣(購物)/一般購物詐欺(偽稱買賣)",
                        "投資詐欺",
                        "猜猜我是誰",
                        "假愛情交友",
                        "盜(冒)用好友身分",
                        "遊戲點數(含虛擬寶物)詐欺",
                        "海外打工詐騙",
                        // 【關鍵】將"其他"選項定義為一個物件
                        { 
                            label: "其他詐騙手法", 
                            hasTextInput: true, // 標記它需要一個文字輸入框
                            placeholder: "請詳述其他手法..." // 輸入框的提示文字
                        }
                    ]
                }
            ]
         },
        { subCategory: " 詐騙事件", eventName: "校屬人員遭電腦網路詐騙事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'fraud_methods',
                    label: '詐騙手法 (可複選):',
                    type: 'checkbox',
                    // 【修改這裡】options 陣列現在混合了字串和物件
                    options: [
                        "解除分期付款詐騙(ATM)",
                        "假網路拍賣(購物)/一般購物詐欺(偽稱買賣)",
                        "投資詐欺",
                        "猜猜我是誰",
                        "假愛情交友",
                        "盜(冒)用好友身分",
                        "遊戲點數(含虛擬寶物)詐欺",
                        "海外打工詐騙",
                        // 【關鍵】將"其他"選項定義為一個物件
                        { 
                            label: "其他詐騙手法", 
                            hasTextInput: true, // 標記它需要一個文字輸入框
                            placeholder: "請詳述其他手法..." // 輸入框的提示文字
                        }
                    ]
                }
            ] },
        { subCategory: "其他校園安全維護事件", eventName: "其他校園安全維護事件", reportType: 'general' },
        { subCategory: "其他校園安全維護事件", eventName: "受犬隻攻擊事件", reportType: 'general' },
        { subCategory: "疑涉犯兒童及少年性剝削防制條例第4章所定之罪", eventName: "疑涉犯兒童及少年性剝削防制條例第4章所定之罪", reportType: 'general' }
    ],
    "暴力事件與偏差行為": [
        { subCategory: "疑似霸凌事件", eventName: "知悉疑似生對生反擊型霸凌", reportType: 'general' },
        { subCategory: "疑似霸凌事件", eventName: "知悉疑似生對生肢體霸凌", reportType: 'general' },
        { subCategory: "疑似霸凌事件", eventName: "知悉疑似生對生關係霸凌", reportType: 'general' },
        { subCategory: "疑似霸凌事件", eventName: "知悉疑似生對生言語霸凌", reportType: 'general' },
        { subCategory: "疑似霸凌事件", eventName: "知悉疑似生對生網路霸凌", reportType: 'general' },
        { subCategory: "霸凌事件", eventName: "確認為反擊型霸凌", reportType: 'general' },
        { subCategory: "霸凌事件", eventName: "確認為肢體霸凌", reportType: 'general' },
        { subCategory: "霸凌事件", eventName: "確認為關係霸凌", reportType: 'general' },
        { subCategory: "霸凌事件", eventName: "確認為言語霸凌", reportType: 'general' },
        { subCategory: "霸凌事件", eventName: "確認為網路霸凌", reportType: 'general' },
        { subCategory: "暴力偏差行為", eventName: "械鬥兇殺事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'violence_type1-1', // 欄位的唯一ID，會當作JSON的key
                    label: '本事件是否準用「校園霸凌防制準則」第71條規定辦理？<br><small class="text-muted fw-normal">一、本案依「校園霸凌防制準則」(以下稱本準則)第71條規定，準用本準則檢舉、審查、調和、調查及處理相關規定辦理，後續事件處理情形請至校園霸凌事件管制系統完成案件管制填報。<br>二、學校知悉或接獲檢舉學生疑似有違法或不當行為，經查證後，教師及學校應依據本準則第21條規定，對該學生採取措施：提供適當心理諮商與輔導、採取適當管教措施、移送權責單位依法定程序予以懲處及其他適當措施。</small>', 
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'violence_type1-2', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "暴力偏差行為", eventName: "幫派鬥毆事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'violence_type2', // 欄位的唯一ID，會當作JSON的key
                    label: '本事件是否準用「校園霸凌防制準則」第72條規定辦理？<br><small class="text-muted fw-normal">一、本案依「校園霸凌防制準則」(以下稱本準則)第71條規定，準用本準則檢舉、審查、調和、調查及處理相關規定辦理，後續事件處理情形請至校園霸凌事件管制系統完成案件管制填報。<br>二、學校知悉或接獲檢舉學生疑似有違法或不當行為，經查證後，教師及學校應依據本準則第22條規定，對該學生採取措施：提供適當心理諮商與輔導、採取適當管教措施、移送權責單位依法定程序予以懲處及其他適當措施。</small>', 
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "暴力偏差行為", eventName: "一般鬥毆事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'violence_type3-1', // 欄位的唯一ID，會當作JSON的key
                    label: '本事件是否準用「校園霸凌防制準則」第73條規定辦理？<br><small class="text-muted fw-normal">一、本案依「校園霸凌防制準則」(以下稱本準則)一、本案依「校園霸凌防制準則」(以下稱本準則)第71條規定，準用本準則檢舉、審查、調和、調查及處理相關規定辦理，後續事件處理情形請至校園霸凌事件管制系統完成案件管制填報。<br>二、學校知悉或接獲檢舉學生疑似有違法或不當行為，經查證後，教師及學校應依據本準則第23條規定，對該學生採取措施：提供適當心理諮商與輔導、採取適當管教措施、移送權責單位依法定程序予以懲處及其他適當措施。</small>', 
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'violence_type3-2', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "暴力偏差行為", eventName: "飆車事件", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉殺人事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'illegal_type1', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "疑涉違法事件", eventName: "疑涉強盜搶奪", reportType: 'general',
            conditionalFields: [
                {
                    id: 'illegal_type2', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]    },
        { subCategory: "疑涉違法事件", eventName: "疑涉恐嚇勒索", reportType: 'general',
            conditionalFields: [
                {
                    id: 'illegal_type3', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]    },
        { subCategory: "疑涉違法事件", eventName: "疑涉擄人綁架", reportType: 'general',
            conditionalFields: [
                {
                    id: 'illegal_type4', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生對學生:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]    },
        { subCategory: "疑涉違法事件", eventName: "疑涉偷竊案件", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉賭博事件", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉及槍砲彈藥刀械管制事件", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉妨害秩序、公務", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉妨害家庭", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "疑涉縱火、破壞事件", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "電腦網路詐騙犯罪案件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'fraud_methods',
                    label: '詐騙手法 (可複選):',
                    type: 'checkbox',
                    // 【修改這裡】options 陣列現在混合了字串和物件
                    options: [
                        "解除分期付款詐騙(ATM)",
                        "假網路拍賣(購物)/一般購物詐欺(偽稱買賣)",
                        "投資詐欺",
                        "猜猜我是誰",
                        "假愛情交友",
                        "盜(冒)用好友身分",
                        "遊戲點數(含虛擬寶物)詐欺",
                        "海外打工詐騙",
                        // 【關鍵】將"其他"選項定義為一個物件
                        { 
                            label: "其他詐騙手法", 
                            hasTextInput: true, // 標記它需要一個文字輸入框
                            placeholder: "請詳述其他手法..." // 輸入框的提示文字
                        }
                    ]
                }
            ] },
        { subCategory: "疑涉違法事件", eventName: "電子菸", reportType: 'general' },
        { subCategory: "疑涉違法事件", eventName: "其他違法事件", reportType: 'general' },
        { subCategory: "藥物濫用事件", eventName: "疑涉及違反毒品危害防制條例", reportType: 'general' },
        { subCategory: "干擾校園安全及事務", eventName: "學生騷擾各級學校及幼兒園典禮事件", reportType: 'general' },
        { subCategory: "干擾校園安全及事務", eventName: "學生騷擾教學事件", reportType: 'general' },
        { subCategory: "干擾校園安全及事務", eventName: "入侵、破壞各級學校及幼兒園資訊系統", reportType: 'general' },
        { subCategory: "干擾校園安全及事務", eventName: "學生集體作弊", reportType: 'general' },
        { subCategory: "干擾校園安全及事務", eventName: "離家出走未就學", reportType: 'general' },
        { subCategory: "其他校園暴力或偏差行為", eventName: "其他校園暴力或偏差行為", reportType: 'general',
            conditionalFields: [
                {
                    id: 'other_violence_type1', // 欄位的唯一ID，會當作JSON的key
                    label: '本事件是否準用「校園霸凌防制準則」第72條規定辦理？<br><small class="text-muted fw-normal">一、本案依「校園霸凌防制準則」(以下稱本準則)第71條規定，準用本準則檢舉、審查、調和、調查及處理相關規定辦理，後續事件處理情形請至校園霸凌事件管制系統完成案件管制填報。<br>二、學校知悉或接獲檢舉學生疑似有違法或不當行為，經查證後，教師及學校應依據本準則第22條規定，對該學生採取措施：提供適當心理諮商與輔導、採取適當管教措施、移送權責單位依法定程序予以懲處及其他適當措施。</small>', 
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]    },
        { subCategory: "其他校園暴力或偏差行為", eventName: "幫派介入校園", reportType: 'general' }
    ],
    "管教衝突事件": [
        { subCategory: "親師生衝突事件", eventName: "師長與學生間衝突事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'teacher_student_conflict1', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生打老師:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]  },
        { subCategory: "親師生衝突事件", eventName: "師長與家長間衝突事件", reportType: 'general',
            conditionalFields: [
                {
                    id: 'teacher_student_conflict1', // 欄位的唯一ID，會當作JSON的key
                    label: '是否為學生打老師:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                }
            ]   },
        { subCategory: "校務行政管教衝突事件", eventName: "行政人員與家長間衝突事件", reportType: 'general' },
        { subCategory: "校務行政管教衝突事件", eventName: "行政人員與學生間衝突事件", reportType: 'general' },
        { subCategory: "其他有關管教衝突事件", eventName: "其他有關管教衝突事件", reportType: 'general' },
        { subCategory: "教師不當管教造成學生身心嚴重侵害之確認事件", eventName: "教師體罰造成學生身心嚴重侵害之確認事件", reportType: 'statutory' },
        { subCategory: "教師不當管教造成學生身心嚴重侵害之確認事件", eventName: "教師其他違法處罰造成學生身心嚴重侵害之確認事件", reportType: 'statutory' },
        { subCategory: "教師不當管教造成學生身心嚴重侵害之疑似事件", eventName: "教師體罰造成學生身心嚴重侵害之疑似事件", reportType: 'statutory' },
        { subCategory: "教師不當管教造成學生身心嚴重侵害之疑似事件", eventName: "教師其他違法處罰造成學生身心嚴重侵害之疑似事件", reportType: 'statutory' },
        { subCategory: "教師不當管教造成學生身心輕微侵害事件", eventName: "教師體罰造成學生身心輕微侵害事件", reportType: 'statutory' },
        { subCategory: "教師不當管教造成學生身心輕微侵害事件", eventName: "教師違法處罰造成學生身心輕微侵害事件", reportType: 'statutory' },
        { subCategory: "其他教師不當管教學生事件(非體罰或違法處罰)", eventName: "其他教師不當管教學生事件(非體罰或違法處罰)", reportType: 'general' },
        { subCategory: "疑似校長及教職員工對學生霸凌事件", eventName: "知悉疑似校長及教職員工對學生反擊型霸凌", reportType: 'statutory' },
        { subCategory: "疑似校長及教職員工對學生霸凌事件", eventName: "知悉疑似校長及教職員工對學生肢體霸凌", reportType: 'statutory' },
        { subCategory: "疑似校長及教職員工對學生霸凌事件", eventName: "知悉疑似校長及教職員工對學生關係霸凌", reportType: 'statutory' },
        { subCategory: "疑似校長及教職員工對學生霸凌事件", eventName: "知悉疑似校長及教職員工對學生言語霸凌", reportType: 'statutory' },
        { subCategory: "疑似校長及教職員工對學生霸凌事件", eventName: "知悉疑似校長及教職員工對學生網路霸凌", reportType: 'statutory' },
        { subCategory: "校長及教職員工對學生霸凌事件", eventName: "確認為反擊型霸凌", reportType: 'general' },
        { subCategory: "校長及教職員工對學生霸凌事件", eventName: "確認為肢體霸凌", reportType: 'statutory' },
        { subCategory: "校長及教職員工對學生霸凌事件", eventName: "確認為關係霸凌", reportType: 'statutory' },
        { subCategory: "校長及教職員工對學生霸凌事件", eventName: "確認為言語霸凌", reportType: 'statutory' },
        { subCategory: "校長及教職員工對學生霸凌事件", eventName: "確認為網路霸凌", reportType: 'statutory' }
    ],
    "兒童少年保護事件(未滿18歲)": [
        { subCategory: "校園性別事件", eventName: "知悉疑似18歲以下性侵害事件(性別平等教育法或非屬性別平等教育法)", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event1-1', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'social_affairs_report_time1-2', // 唯一ID
                    label: '社政單位通報時間',           // 標籤
                    type: 'datetime',                // 【關鍵】新的類型
                    description: '（若無則免填）',      // 說明文字
                    required: false
                },
                {
                    id: 'gender_event1-3', // 唯一ID
                    label: '社政單位通報編號',           // 標籤
                    type: 'text',   
                    placeholder: '請輸入通報編號...',
                    required: false
                }
            ]   },
        { subCategory: "校園性別事件", eventName: "知悉疑似18歲以下性騷擾事件(性別平等教育法或非屬性別平等教育法)", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event2-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                },
                {
                    id: 'compusgender_event2-2', // 欄位的唯一ID，會當作JSON的key
                    label: '數位/網路性別暴力事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'compusgender_event2-3', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'social_affairs_report_time2-4', // 唯一ID
                    label: '社政單位通報時間',           // 標籤
                    type: 'datetime',                // 【關鍵】新的類型
                    description: '（若無則免填）'      // 說明文字
                },
                {
                    id: 'gender_event2-5', // 唯一ID
                    label: '社政單位通報編號',           // 標籤
                    type: 'text',   
                    placeholder: '請輸入通報編號...'             
                }
            ]    },
        { subCategory: "校園性別事件", eventName: "知悉疑似18歲以下性霸凌事件", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event3-1', // 欄位的唯一ID，會當作JSON的key
                    label: '數位/網路性別暴力事件類型:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'compusgender_event3-2', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'social_affairs_report_time3-3', // 唯一ID
                    label: '社政單位通報時間',           // 標籤
                    type: 'datetime',                // 【關鍵】新的類型
                    description: '（若無則免填）'      // 說明文字
                },
                {
                    id: 'gender_event3-4', // 唯一ID
                    label: '社政單位通報編號',           // 標籤
                    type: 'text',   
                    placeholder: '請輸入通報編號...'             
                }
            ]    },
        { subCategory: "校園性別事件", eventName: "知悉疑似校長或教職員工違反與性或性別有關之專業倫理行為", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event4-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                },
                {
                    id: 'compusgender_event4-2', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'social_affairs_report_time4-3', // 唯一ID
                    label: '社政單位通報時間',           // 標籤
                    type: 'datetime',                // 【關鍵】新的類型
                    description: '（若無則免填）'      // 說明文字
                },
                {
                    id: 'gender_event4-4', // 唯一ID
                    label: '社政單位通報編號',           // 標籤
                    type: 'text',   
                    placeholder: '請輸入通報編號...'             
                }
            ]    },
        { subCategory: "藥物濫用事件", eventName: "知悉兒少施用毒品、非法施用管制藥品或其他有害身心健康之物質", reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event5-1', // 欄位的唯一ID，會當作JSON的key
                    label: '通報社政單位:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是",
                        "否"
                    ]
                },
                {
                    id: 'social_affairs_report_time5-2', // 唯一ID
                    label: '社政單位通報時間',           // 標籤
                    type: 'datetime',                // 【關鍵】新的類型
                    description: '（若無則免填）'      // 說明文字
                },
                {
                    id: 'gender_event5-3', // 唯一ID
                    label: '社政單位通報編號',           // 標籤
                    type: 'text',   
                    placeholder: '請輸入通報編號...'             
                }
            ]    },
        { subCategory: "兒少保護事件", eventName: "強迫、引誘、容留或媒介兒童及少年為猥褻行為或性交", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉屬兒童及少年福利與權益保障法第五十六條第一項所定應立即給予保護、安置或為其他處置，其生命、身體或自由有立即之危險或有危險之虞者", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉有其他對兒少或利用兒少犯罪或為不正當之行為", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉供應兒少刀械、槍砲、彈藥或其他危險物品", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉兒少遭遺棄", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉兒少遭身心虐待", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉兒少遭拐騙、綁架、買賣、質押", reportType: 'statutory' },
        {
            subCategory: "兒少保護事件", eventName: "知悉兒少(充當)酒家、特種咖啡茶室、成人用品零售店、限制級電子遊戲場及其他涉及賭博、色情、暴力等經主管機關認定足以危害其身心健康之場所侍應", reportType: 'statutory'
        },
        { subCategory: "兒少保護事件", eventName: "知悉兒少被利用從事有害健康等危害性活動或欺騙之行為", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉利用身心障礙或特殊形體兒少供人參觀", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉利用兒少行乞", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉兒少遭剝奪或妨礙兒少接受國民教育之機會", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉強迫兒少婚嫁", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉兒少遭利用拍攝或錄製暴力、血腥、色情、猥褻或其他有害兒少身心健康之出版品、圖畫、錄影節目帶、影片、光碟、磁片、電子訊號、遊戲軟體、網際網路內容或其他物品", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉帶領或誘使兒少進入有礙其身心健康之場所", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉對於六歲以下兒童或需要特別看護之兒少，使其獨處或由不適當之人代為照顧", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "迫使或誘使兒少處於對其生命、身體易發生立即危險或傷害之環境", reportType: 'statutory' },
        { subCategory: "兒少保護事件", eventName: "知悉強迫、引誘、容留或媒介兒童及少年為自殺行為", reportType: 'statutory' },
        { subCategory: "兒童及少年遭性剝削或疑似遭受性剝削事件", eventName: "使兒童或少年為有對價之性交或猥褻行為", reportType: 'statutory' },
        { subCategory: "兒童及少年遭性剝削或疑似遭受性剝削事件", eventName: "利用兒童或少年為性交、猥褻之行為，以供人觀覽", reportType: 'statutory' },
        { subCategory: "兒童及少年遭性剝削或疑似遭受性剝削事件", eventName: "使兒童或少年坐檯陪酒或涉及色情之伴遊、伴唱、伴舞等侍應工作", reportType: 'statutory' },
        { subCategory: "家庭暴力事件", eventName: "知悉疑似家庭暴力情事" , reportType: 'statutory',
            conditionalFields: [
                {
                    id: 'compusgender_event27-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                }
            ]   },
        { subCategory: "家庭暴力事件", eventName: "知悉兒少目睹家庭暴力(新知悉案件請通報家庭暴力事件)", reportType: 'statutory' },
        { subCategory: "家庭暴力事件", eventName: "校園親密關係暴力事件（屬家庭暴力防治法第63-1條，學生間發生未同居親密關係暴力事件）", reportType: 'statutory' ,
            conditionalFields: [
                {
                    id: 'compusgender_event27-1', // 欄位的唯一ID，會當作JSON的key
                    label: '跟蹤騷擾防制法事件:', // 欄位標籤
                    type: 'radio', // 欄位類型 (radio, text, etc.)
                    options: [ // 選項 (僅 radio/select/checkbox 需要)                      
                        "是(有跟蹤騷擾防制法第3條之跟蹤騷擾行為)",
                        "否 (一般性騷擾事件【含性別平等教育法、性別工作平等法及性騷擾防治法】)"
                    ]
                }
            ]   },
        { subCategory: "其他兒少保護事件", eventName: "執行業務時知悉兒童及少年家庭遭遇經濟、教養、婚姻、醫療等問題，致兒童及少年有未獲適當照顧之虞", reportType: 'statutory' },
        { subCategory: "其他兒少保護事件", eventName: "知悉父母、監護人或其他實際照顧兒少之人使兒童獨處於易發生危險或傷害之環境", reportType: 'statutory' }
    ],
    "天然災害事件": [
        { subCategory: "天然災害", eventName: "風災", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "水災", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "震災(含土壤液化)", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "土石流災害", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "雷擊", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "核災", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "海嘯", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "旱災", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "寒害", reportType: 'statutory' },
        { subCategory: "天然災害", eventName: "火山災害", reportType: 'statutory' },
        { subCategory: "其他重大災害", eventName: "其他重大災害", reportType: 'statutory' },
        { subCategory: "環境災害", eventName: "紅火蟻", reportType: 'general' },
        { subCategory: "環境災害", eventName: "沙塵事件", reportType: 'general' },
        { subCategory: "環境災害", eventName: "一般空氣汙染", reportType: 'general' },
        { subCategory: "環境災害", eventName: "秋行軍蟲", reportType: 'general' },
        { subCategory: "環境災害", eventName: "荔枝椿象", reportType: 'general' }
    ],
    "疾病事件": [
        { subCategory: "法定傳染病", eventName: "結核病", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "腸病毒感染併發重症", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "流感併發重症", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "水痘併發症", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "登革熱", reportType: 'statutory' },
        {
            subCategory: "法定傳染病", eventName: "其他：請參閱衛生福利部疾病管制署網站公布五類法定傳染病<br>(網址：https://www.cdc.gov.tw/professional/disease.aspx?treeid=beac9c103df952c4&nowtreeid=6b7f57aafde15f54)", reportType: 'statutory'
        },
        { subCategory: "法定傳染病", eventName: "新型A型流感", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "流行性腮腺炎", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "恙蟲病", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "百日咳", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "Q熱", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "嚴重特殊傳染性肺炎(COVID-19、新冠肺炎)", reportType: 'statutory' },
        { subCategory: "法定傳染病", eventName: "猴痘", reportType: 'statutory' },
        { subCategory: "一般傳染病", eventName: "紅眼症", reportType: 'general' },
        { subCategory: "一般傳染病", eventName: "腸病毒(非併發重症，如出現手足口病或疱疹性咽峽炎)", reportType: 'general' },
        { subCategory: "一般傳染病", eventName: "水痘", reportType: 'general' },
        { subCategory: "一般傳染病", eventName: "病毒性腸胃炎(如輪狀病毒、諾羅病毒及腺病毒，出現腹瀉症狀)", reportType: 'general' }
    ],
    "其他事件": [
        { subCategory: "校務相關問題", eventName: "教職員間之問題", reportType: 'general' },
        { subCategory: "校務相關問題", eventName: "總務問題", reportType: 'general' },
        { subCategory: "校務相關問題", eventName: "人事問題", reportType: 'general' },
        { subCategory: "校務相關問題", eventName: "行政問題", reportType: 'general' },
        { subCategory: "校務相關問題", eventName: "教務問題", reportType: 'general' },
        { subCategory: "其他的問題", eventName: "其他問題", reportType: 'general' },
        { subCategory: "其他的問題", eventName: "其他問題(動物感染狂犬病)", reportType: 'general' }
    ]
};