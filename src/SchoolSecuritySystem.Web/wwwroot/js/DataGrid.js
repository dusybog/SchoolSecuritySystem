// DataGrid.js
export class DataGrid {
    constructor(config) {
        this.container = document.getElementById(config.containerId);
        this.title = config.title || "";
        this.apiUrl = config.apiUrl;
        this.staticData = config.data || null;
        this.columns = config.columnMapping;
        this.actionHtml = config.actionHtml;
        this.rowClickUrl = config.rowClickUrl;
        this.pageSize = config.pageSize || 10;
        this.colspan = Object.keys(this.columns).length + (this.actionHtml ? 1 : 0);
        this.currentPage = 1;

        this.bindEvents();
    }

    setData(newData) {
        this.staticData = newData;
        this.load(1);
    }

    async load(page = 1) {
        this.currentPage = page;
        this.renderTable(
            `<tr><td colspan="${this.colspan}" class="text-center p-4 text-secondary">⏳ 載入資料中...</td></tr>`
        );

        try {
            let items = [];
            let totalPages = 0;

            if (this.apiUrl) {
                const urlObj = new URL(this.apiUrl, window.location.origin);
                 urlObj.searchParams.append('page', this.currentPage);
                 urlObj.searchParams.append('pageSize', this.pageSize);

                const response = await fetch(urlObj.toString());
                if (!response.ok)
                    throw new Error(`伺服器回應錯誤 (${response.status})`);

                const result = await response.json();

                if (Array.isArray(result)) {
                    items = result;
                    totalPages = 0;
                } else if (result && typeof result === "object") {
                    items = result.data || result.items || [];
                    totalPages = result.totalPages !== undefined ? result.totalPages : 0;
                }
            } else if (this.staticData) {
                if (!Array.isArray(this.staticData))
                    throw new Error("傳入的靜態資料格式錯誤，必須為陣列 (Array)。");
                if (this.pageSize > 0 && this.staticData.length > this.pageSize) {
                    totalPages = Math.ceil(this.staticData.length / this.pageSize);
                    const startIndex = (this.currentPage - 1) * this.pageSize;
                    items = this.staticData.slice(startIndex, startIndex + this.pageSize);
                } else {
                    totalPages = 0;
                    items = this.staticData;
                }
            } else {
                throw new Error("必須提供 apiUrl 或 data 其中一項設定！");
            }

            this.renderTable(
                this.generateRows(items),
                this.generatePagination(totalPages)
            );
        } catch (error) {
            this.renderTable(
                `<tr><td colspan="${this.colspan}" class="text-center p-4 text-danger bg-danger bg-opacity-10">❌ 載入失敗：${error.message}</td></tr>`
            );
        }
    }

    renderTable(tbodyContent, paginationHtml = "") {
        let titleHtml = "";
        if (this.title) {
            titleHtml = `<div class="fs-5 fw-bold text-dark mb-3 ps-2 border-start border-4 border-primary">${this.title}</div>`;
        }

        // 🌟 加上 data-grid-wrapper class
        const tableWrapper = `
      <div class="data-grid-wrapper table-responsive border rounded-3 bg-white shadow-sm mb-0">
        ${this.generateHeader()}
        ${tbodyContent}
        </tbody></table>
      </div>
    `;

        this.container.innerHTML = titleHtml + tableWrapper + paginationHtml;
    }

    generateHeader() {
        // 🌟 加上 data-grid-table class
        let html = `
      <table class="data-grid-table table table-hover mb-0">
        <thead class="table-light">
          <tr>
    `;
        for (const key in this.columns) {
            const colDef = this.columns[key];
            const label = typeof colDef === "object" ? colDef.label : colDef;
            html += `<th class="px-3 py-3 text-start fw-semibold text-secondary text-nowrap">${label}</th>`;
        }
        if (this.actionHtml) {
            html += `<th class="px-3 py-3 text-center fw-semibold text-secondary text-nowrap">操作</th>`;
        }
        html += `</tr></thead><tbody>`;
        return html;
    }

    generateRows(items) {
        if (items.length === 0) {
            return `<tr><td colspan="${this.colspan}" class="text-center p-5 text-secondary">目前沒有資料</td></tr>`;
        }

        let html = "";
        items.forEach((row) => {
            let url = "";
            if (typeof this.rowClickUrl === "function") {
                url = this.rowClickUrl(row);
            } else if (this.rowClickUrl) {
                url = this.rowClickUrl.replace("{id}", row.id);
            }

            const trClass = url ? "clickable-row" : "";
            const trStyle = url ? "cursor: pointer;" : "";

            html += `<tr class="${trClass}" data-url="${url}" style="${trStyle}">`;

            for (const key in this.columns) {
                let value =
                    row[key] !== undefined && row[key] !== null ? row[key] : "-";

                const colDef = this.columns[key];
                // 🌟 取得欄位標題，用來放在 data-label 裡面
                const label = typeof colDef === "object" ? colDef.label : colDef;

                if (
                    typeof colDef === "object" &&
                    typeof colDef.formatter === "function"
                ) {
                    value = colDef.formatter(value, row);
                }

                // 🌟 補上 data-label 屬性供手機版 RWD 使用
                html += `<td class="px-3 py-3 align-middle" data-label="${label}">${value}</td>`;
            }

            if (this.actionHtml) {
                const actionContent =
                    typeof this.actionHtml === "function"
                        ? this.actionHtml(row)
                        : this.actionHtml;
                // 🌟 操作欄位也補上 data-label
                html += `<td class="action-cell px-3 py-3 text-center align-middle" data-label="操作">${actionContent}</td>`;
            }

            html += `</tr>`;
        });
        return html;
    }

    generatePagination(totalPages) {
        if (totalPages <= 1) return "";

        let pages = [];
        const current = this.currentPage;

        if (totalPages <= 6) {
            for (let i = 1; i <= totalPages; i++) pages.push(i);
        } else {
            if (current <= 3) {
                pages = [1, 2, 3, 4, "...", totalPages];
            } else if (current >= totalPages - 2) {
                pages = [
                    1,
                    "...",
                    totalPages - 3,
                    totalPages - 2,
                    totalPages - 1,
                    totalPages,
                ];
            } else {
                pages = [
                    1,
                    "...",
                    current - 1,
                    current,
                    current + 1,
                    "...",
                    totalPages,
                ];
            }
        }

        let html = `<div class="mt-4 d-flex justify-content-center align-items-center flex-wrap gap-4">`;

        html += `<ul class="pagination mb-0">`;
        pages.forEach((p) => {
            if (p === "...") {
                html += `<li class="page-item disabled"><span class="page-link border-0 text-secondary bg-transparent">...</span></li>`;
            } else {
                const activeClass = p === this.currentPage ? "active" : "";
                html += `<li class="page-item ${activeClass}"><button class="page-link page-btn shadow-none" data-page="${p}">${p}</button></li>`;
            }
        });
        html += `</ul>`;

        html += `
      <div class="d-flex align-items-center gap-2 text-secondary small">
        <span>前往</span>
        <input type="number" class="form-control form-control-sm text-center shadow-none page-jump-input" 
               min="1" max="${totalPages}" value="${this.currentPage}" title="輸入頁碼後按 Enter 跳轉" style="width: 70px;">
        <span>/ ${totalPages} 頁 <span class="text-muted">(按 Enter)</span></span>
      </div>
    `;

        html += `</div>`;
        return html;
    }

    handlePageJump(input) {
        if (!input) return;

        const targetPage = parseInt(input.value);
        const maxPage = parseInt(input.getAttribute("max"));

        if (isNaN(targetPage) || targetPage < 1 || targetPage > maxPage) {
            alert(`⚠️ 請輸入 1 到 ${maxPage} 之間的有效頁碼！`);
            input.value = this.currentPage;
            return;
        }

        if (targetPage !== this.currentPage) {
            this.load(targetPage);
        }
    }

    bindEvents() {
        this.container.addEventListener("click", (e) => {
            const pageBtn = e.target.closest(".page-btn");
            if (pageBtn) {
                const page = parseInt(pageBtn.dataset.page);
                if (page !== this.currentPage) this.load(page);
                return;
            }

            const tr = e.target.closest(".clickable-row");
            const actionCell = e.target.closest(".action-cell");

            const isInteractiveElement = e.target.closest("button, a, input");

            if (tr && !actionCell && !isInteractiveElement) {
                const targetUrl = tr.dataset.url;
                if (targetUrl) {
                    window.open(targetUrl, "_blank");
                }
            }
        });

        this.container.addEventListener("keydown", (e) => {
            if (e.key === "Enter" && e.target.classList.contains("page-jump-input")) {
                e.preventDefault();
                this.handlePageJump(e.target);
            }
        });
    }
}