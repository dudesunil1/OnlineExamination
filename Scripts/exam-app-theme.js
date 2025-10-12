/* ========================================
   EXAM APP THEME JAVASCRIPT
   Professional Enterprise Functionality
   ======================================== */

class ExamAppTheme {
    constructor() {
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.initializeComponents();
        this.setupFormValidation();
        this.setupDataTables();
        this.setupModals();
        this.setupNotifications();
        this.setupSidebar();
        this.setupResponsive();
    }

    /* ========================================
       EVENT LISTENERS
       ======================================== */

    setupEventListeners() {
        document.addEventListener('DOMContentLoaded', () => {
            this.initializeComponents();
        });

        // Global click handler
        document.addEventListener('click', (e) => {
            this.handleGlobalClick(e);
        });

        // Form submissions
        document.addEventListener('submit', (e) => {
            this.handleFormSubmission(e);
        });

        // Window resize
        window.addEventListener('resize', () => {
            this.handleResize();
        });
    }

    /* ========================================
       COMPONENT INITIALIZATION
       ======================================== */

    initializeComponents() {
        this.initializeTooltips();
        this.initializeDropdowns();
        this.initializeAccordions();
        this.initializeTabs();
        this.initializeFileUploads();
        this.initializeDatePickers();
        this.initializeSearch();
        this.initializeProgressBars();
    }

    /* ========================================
       FORM VALIDATION
       ======================================== */

    setupFormValidation() {
        const forms = document.querySelectorAll('form[data-validate]');
        
        forms.forEach(form => {
            const inputs = form.querySelectorAll('input, select, textarea');
            
            inputs.forEach(input => {
                // Real-time validation
                input.addEventListener('blur', () => {
                    this.validateField(input);
                });

                input.addEventListener('input', () => {
                    this.clearFieldError(input);
                });
            });
        });
    }

    validateField(field) {
        const value = field.value.trim();
        const rules = this.getValidationRules(field);
        let isValid = true;
        let message = '';

        // Required validation
        if (rules.required && !value) {
            isValid = false;
            message = 'This field is required.';
        }

        // Email validation
        if (value && rules.email && !this.isValidEmail(value)) {
            isValid = false;
            message = 'Please enter a valid email address.';
        }

        // Phone validation
        if (value && rules.phone && !this.isValidPhone(value)) {
            isValid = false;
            message = 'Please enter a valid phone number.';
        }

        // Length validation
        if (value && rules.minLength && value.length < rules.minLength) {
            isValid = false;
            message = `Minimum length is ${rules.minLength} characters.`;
        }

        if (value && rules.maxLength && value.length > rules.maxLength) {
            isValid = false;
            message = `Maximum length is ${rules.maxLength} characters.`;
        }

        // Pattern validation
        if (value && rules.pattern && !new RegExp(rules.pattern).test(value)) {
            isValid = false;
            message = rules.patternMessage || 'Invalid format.';
        }

        this.displayFieldValidation(field, isValid, message);
        return isValid;
    }

    getValidationRules(field) {
        const rules = {};
        
        if (field.hasAttribute('required')) rules.required = true;
        if (field.type === 'email') rules.email = true;
        if (field.type === 'tel') rules.phone = true;
        if (field.hasAttribute('minlength')) rules.minLength = parseInt(field.getAttribute('minlength'));
        if (field.hasAttribute('maxlength')) rules.maxLength = parseInt(field.getAttribute('maxlength'));
        if (field.hasAttribute('pattern')) {
            rules.pattern = field.getAttribute('pattern');
            rules.patternMessage = field.getAttribute('data-pattern-message');
        }

        return rules;
    }

    displayFieldValidation(field, isValid, message) {
        this.clearFieldError(field);

        if (message) {
            const errorElement = document.createElement('div');
            errorElement.className = `exam-field-error ${isValid ? 'exam-field-success' : 'exam-field-error'}`;
            errorElement.textContent = message;
            
            field.parentNode.appendChild(errorElement);
            field.classList.add(isValid ? 'exam-field-valid' : 'exam-field-invalid');
        }
    }

    clearFieldError(field) {
        const errorElement = field.parentNode.querySelector('.exam-field-error');
        if (errorElement) {
            errorElement.remove();
        }
        field.classList.remove('exam-field-valid', 'exam-field-invalid');
    }

    /* ========================================
       VALIDATION HELPERS
       ======================================== */

    isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    isValidPhone(phone) {
        const phoneRegex = /^[\+]?[1-9][\d]{0,15}$/;
        return phoneRegex.test(phone.replace(/[\s\-\(\)]/g, ''));
    }

    /* ========================================
       FORM SUBMISSION
       ======================================== */

    handleFormSubmission(e) {
        const form = e.target;
        const submitButton = form.querySelector('button[type="submit"], input[type="submit"]');
        
        if (submitButton) {
            // Show loading state
            this.setButtonLoading(submitButton, true);
            
            // Validate form
            const isValid = this.validateForm(form);
            
            if (!isValid) {
                e.preventDefault();
                this.setButtonLoading(submitButton, false);
                this.showNotification('Please correct the errors in the form.', 'error');
            }
        }
    }

    validateForm(form) {
        const fields = form.querySelectorAll('input, select, textarea');
        let isValid = true;

        fields.forEach(field => {
            if (!this.validateField(field)) {
                isValid = false;
            }
        });

        return isValid;
    }

    setButtonLoading(button, loading) {
        if (loading) {
            button.disabled = true;
            button.dataset.originalText = button.textContent;
            button.innerHTML = '<span class="exam-spinner"></span> Processing...';
        } else {
            button.disabled = false;
            button.textContent = button.dataset.originalText;
        }
    }

    /* ========================================
       DATA TABLES
       ======================================== */

    setupDataTables() {
        const tables = document.querySelectorAll('.exam-table');
        
        tables.forEach(table => {
            this.enhanceTable(table);
        });
    }

    enhanceTable(table) {
        // Add sorting functionality
        const headers = table.querySelectorAll('th[data-sortable]');
        
        headers.forEach(header => {
            header.style.cursor = 'pointer';
            header.addEventListener('click', () => {
                this.sortTable(table, header);
            });
        });

        // Add search functionality
        const searchInput = table.parentNode.querySelector('.exam-table-search');
        if (searchInput) {
            searchInput.addEventListener('input', () => {
                this.filterTable(table, searchInput.value);
            });
        }
    }

    sortTable(table, header) {
        const tbody = table.querySelector('tbody');
        const rows = Array.from(tbody.querySelectorAll('tr'));
        const columnIndex = Array.from(header.parentNode.children).indexOf(header);
        const isAscending = header.classList.contains('exam-sort-asc');

        // Clear existing sort classes
        header.parentNode.querySelectorAll('th').forEach(th => {
            th.classList.remove('exam-sort-asc', 'exam-sort-desc');
        });

        // Sort rows
        rows.sort((a, b) => {
            const aValue = a.children[columnIndex].textContent.trim();
            const bValue = b.children[columnIndex].textContent.trim();
            
            if (isAscending) {
                return bValue.localeCompare(aValue);
            } else {
                return aValue.localeCompare(bValue);
            }
        });

        // Reorder rows in DOM
        rows.forEach(row => tbody.appendChild(row));

        // Update sort indicator
        header.classList.add(isAscending ? 'exam-sort-desc' : 'exam-sort-asc');
    }

    filterTable(table, searchTerm) {
        const tbody = table.querySelector('tbody');
        const rows = tbody.querySelectorAll('tr');

        rows.forEach(row => {
            const text = row.textContent.toLowerCase();
            const matches = text.includes(searchTerm.toLowerCase());
            row.style.display = matches ? '' : 'none';
        });
    }

    /* ========================================
       MODALS
       ======================================== */

    setupModals() {
        // Modal triggers
        document.querySelectorAll('[data-modal-trigger]').forEach(trigger => {
            trigger.addEventListener('click', (e) => {
                e.preventDefault();
                const modalId = trigger.dataset.modalTrigger;
                this.openModal(modalId);
            });
        });

        // Modal close buttons
        document.querySelectorAll('.exam-modal-close').forEach(closeBtn => {
            closeBtn.addEventListener('click', () => {
                this.closeModal(closeBtn.closest('.exam-modal'));
            });
        });

        // Modal backdrop
        document.querySelectorAll('.exam-modal-backdrop').forEach(backdrop => {
            backdrop.addEventListener('click', (e) => {
                if (e.target === backdrop) {
                    this.closeModal(backdrop.querySelector('.exam-modal'));
                }
            });
        });

        // Escape key
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape') {
                const openModal = document.querySelector('.exam-modal-backdrop.show');
                if (openModal) {
                    this.closeModal(openModal.querySelector('.exam-modal'));
                }
            }
        });
    }

    openModal(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            const backdrop = modal.closest('.exam-modal-backdrop');
            backdrop.classList.add('show');
            document.body.style.overflow = 'hidden';
            
            // Focus first input
            const firstInput = modal.querySelector('input, select, textarea');
            if (firstInput) {
                setTimeout(() => firstInput.focus(), 100);
            }
        }
    }

    closeModal(modal) {
        if (modal) {
            const backdrop = modal.closest('.exam-modal-backdrop');
            backdrop.classList.remove('show');
            document.body.style.overflow = '';
        }
    }

    /* ========================================
       NOTIFICATIONS
       ======================================== */

    setupNotifications() {
        // Auto-dismiss notifications
        document.querySelectorAll('.exam-alert[data-auto-dismiss]').forEach(alert => {
            const delay = parseInt(alert.dataset.autoDismiss) || 5000;
            setTimeout(() => {
                this.dismissNotification(alert);
            }, delay);
        });
    }

    showNotification(message, type = 'info', duration = 5000) {
        const notification = document.createElement('div');
        notification.className = `exam-alert exam-alert-${type} exam-notification`;
        notification.innerHTML = `
            <div class="exam-alert-icon">
                <i class="${this.getNotificationIcon(type)}"></i>
            </div>
            <div class="exam-alert-content">
                <div class="exam-alert-message">${message}</div>
            </div>
            <button class="exam-alert-close" onclick="this.parentNode.remove()">
                <i class="fas fa-times"></i>
            </button>
        `;

        // Add to container
        let container = document.querySelector('.exam-notifications-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'exam-notifications-container';
            document.body.appendChild(container);
        }

        container.appendChild(notification);

        // Animate in
        setTimeout(() => notification.classList.add('show'), 100);

        // Auto-dismiss
        if (duration > 0) {
            setTimeout(() => {
                this.dismissNotification(notification);
            }, duration);
        }

        return notification;
    }

    dismissNotification(notification) {
        notification.classList.add('dismissing');
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 300);
    }

    getNotificationIcon(type) {
        const icons = {
            success: 'fas fa-check-circle',
            error: 'fas fa-exclamation-circle',
            warning: 'fas fa-exclamation-triangle',
            info: 'fas fa-info-circle'
        };
        return icons[type] || 'fas fa-info-circle';
    }

    /* ========================================
       SIDEBAR
       ======================================== */

    setupSidebar() {
        const toggleBtn = document.querySelector('.exam-sidebar-toggle');
        const sidebar = document.querySelector('.exam-sidebar');
        const overlay = document.querySelector('.exam-sidebar-overlay');

        console.log('Setting up sidebar:', { toggleBtn, sidebar, overlay });

        if (toggleBtn && sidebar) {
            toggleBtn.addEventListener('click', (e) => {
                e.preventDefault();
                console.log('Sidebar toggle clicked');
                this.toggleSidebar();
            });
        }

        if (overlay) {
            overlay.addEventListener('click', () => {
                console.log('Overlay clicked, closing sidebar');
                this.closeSidebar();
            });
        }

        // Active menu item
        this.setActiveMenuItem();
    }

    toggleSidebar() {
        const sidebar = document.querySelector('.exam-sidebar');
        const overlay = document.querySelector('.exam-sidebar-overlay');
        const mainContent = document.querySelector('.exam-main-content');
        
        console.log('Toggling sidebar:', sidebar.classList.contains('open'));
        
        const isCurrentlyOpen = sidebar.classList.contains('open');
        
        if (isCurrentlyOpen) {
            // Close sidebar
            sidebar.classList.remove('open');
            sidebar.classList.add('closed');
            if (overlay) overlay.classList.remove('show');
            if (mainContent) mainContent.style.marginLeft = '0';
        } else {
            // Open sidebar
            sidebar.classList.remove('closed');
            sidebar.classList.add('open');
            if (overlay) overlay.classList.add('show');
            if (mainContent && window.innerWidth > 1024) {
                mainContent.style.marginLeft = '280px';
            }
        }
        
        console.log('Sidebar state after toggle:', sidebar.classList.contains('open'));
    }

    closeSidebar() {
        const sidebar = document.querySelector('.exam-sidebar');
        const overlay = document.querySelector('.exam-sidebar-overlay');
        const mainContent = document.querySelector('.exam-main-content');
        
        sidebar.classList.remove('open');
        sidebar.classList.add('closed');
        if (overlay) overlay.classList.remove('show');
        if (mainContent) mainContent.style.marginLeft = '0';
    }

    setActiveMenuItem() {
        const currentPath = window.location.pathname;
        const menuItems = document.querySelectorAll('.exam-nav-item');
        
        menuItems.forEach(item => {
            const href = item.getAttribute('href');
            if (href && currentPath.includes(href)) {
                item.classList.add('active');
            }
        });
    }

    /* ========================================
       RESPONSIVE HANDLING
       ======================================== */

    setupResponsive() {
        this.handleResize();
    }

    handleResize() {
        const sidebar = document.querySelector('.exam-sidebar');
        const mainContent = document.querySelector('.exam-main-content');
        
        if (window.innerWidth <= 1024) {
            sidebar.classList.remove('open');
            if (mainContent) {
                mainContent.style.marginLeft = '0';
            }
        } else {
            if (mainContent) {
                mainContent.style.marginLeft = '280px';
            }
        }
    }

    /* ========================================
       COMPONENT INITIALIZERS
       ======================================== */

    initializeTooltips() {
        document.querySelectorAll('[data-tooltip]').forEach(element => {
            element.addEventListener('mouseenter', (e) => {
                this.showTooltip(e.target, e.target.dataset.tooltip);
            });

            element.addEventListener('mouseleave', () => {
                this.hideTooltip();
            });
        });
    }

    showTooltip(element, text) {
        const tooltip = document.createElement('div');
        tooltip.className = 'exam-tooltip';
        tooltip.textContent = text;
        
        document.body.appendChild(tooltip);

        const rect = element.getBoundingClientRect();
        tooltip.style.top = rect.top - tooltip.offsetHeight - 5 + 'px';
        tooltip.style.left = rect.left + (rect.width / 2) - (tooltip.offsetWidth / 2) + 'px';
        
        tooltip.classList.add('show');
    }

    hideTooltip() {
        const tooltip = document.querySelector('.exam-tooltip');
        if (tooltip) {
            tooltip.remove();
        }
    }

    initializeDropdowns() {
        // Handle sidebar menu dropdowns
        document.querySelectorAll('.exam-menu-item.has-dropdown').forEach(item => {
            const link = item.querySelector('.exam-menu-link');
            const submenu = item.querySelector('.sidebar-submenu');
            
            if (link && submenu) {
                link.addEventListener('click', (e) => {
                    e.preventDefault();
                    
                    // Remove activePage from other menu items
                    document.querySelectorAll('.exam-menu-item').forEach(otherItem => {
                        if (otherItem !== item) {
                            otherItem.classList.remove('activePage');
                            const otherSubmenu = otherItem.querySelector('.sidebar-submenu');
                            if (otherSubmenu) {
                                otherSubmenu.style.display = 'none';
                            }
                        }
                    });
                    
                    // Toggle current menu item
                    item.classList.toggle('activePage');
                    const isActive = item.classList.contains('activePage');
                    
                    if (isActive) {
                        submenu.style.display = 'block';
                    } else {
                        submenu.style.display = 'none';
                    }
                });
            }
        });
        
        // Handle top navbar dropdowns
        document.querySelectorAll('.exam-dropdown').forEach(dropdown => {
            const trigger = dropdown.querySelector('.exam-dropdown-trigger');
            const menu = dropdown.querySelector('.exam-dropdown-menu');

            if (trigger && menu) {
                trigger.addEventListener('click', (e) => {
                    e.stopPropagation();
                    this.toggleDropdown(dropdown);
                });
            }
        });

        // Close dropdowns when clicking outside
        document.addEventListener('click', () => {
            document.querySelectorAll('.exam-dropdown.open').forEach(dropdown => {
                dropdown.classList.remove('open');
            });
        });
    }

    toggleDropdown(dropdown) {
        dropdown.classList.toggle('open');
    }

    initializeAccordions() {
        document.querySelectorAll('.exam-accordion-header').forEach(header => {
            header.addEventListener('click', () => {
                this.toggleAccordion(header);
            });
        });
    }

    toggleAccordion(header) {
        const accordion = header.parentNode;
        const content = header.nextElementSibling;
        const isOpen = accordion.classList.contains('open');

        // Close all accordions in the same group
        const group = accordion.closest('.exam-accordion-group');
        if (group) {
            group.querySelectorAll('.exam-accordion-item').forEach(item => {
                item.classList.remove('open');
                item.querySelector('.exam-accordion-content').style.maxHeight = '0';
            });
        }

        // Toggle current accordion
        if (!isOpen) {
            accordion.classList.add('open');
            content.style.maxHeight = content.scrollHeight + 'px';
        }
    }

    initializeTabs() {
        document.querySelectorAll('.exam-tab-nav').forEach(tabNav => {
            const tabs = tabNav.querySelectorAll('.exam-tab');
            
            tabs.forEach(tab => {
                tab.addEventListener('click', () => {
                    this.switchTab(tab);
                });
            });
        });
    }

    switchTab(activeTab) {
        const tabGroup = activeTab.closest('.exam-tab-group');
        const tabNav = tabGroup.querySelector('.exam-tab-nav');
        const tabContent = tabGroup.querySelector('.exam-tab-content');
        
        // Update active tab
        tabNav.querySelectorAll('.exam-tab').forEach(tab => {
            tab.classList.remove('active');
        });
        activeTab.classList.add('active');

        // Show corresponding content
        const targetId = activeTab.dataset.tabTarget;
        tabContent.querySelectorAll('.exam-tab-panel').forEach(panel => {
            panel.classList.remove('active');
        });
        
        const targetPanel = tabContent.querySelector(`#${targetId}`);
        if (targetPanel) {
            targetPanel.classList.add('active');
        }
    }

    initializeFileUploads() {
        document.querySelectorAll('input[type="file"]').forEach(input => {
            input.addEventListener('change', (e) => {
                this.handleFileUpload(e.target);
            });
        });
    }

    handleFileUpload(input) {
        const files = input.files;
        const preview = input.parentNode.querySelector('.exam-file-preview');
        
        if (preview && files.length > 0) {
            preview.innerHTML = '';
            
            Array.from(files).forEach(file => {
                const fileItem = document.createElement('div');
                fileItem.className = 'exam-file-item';
                fileItem.innerHTML = `
                    <div class="exam-file-icon">
                        <i class="fas fa-file"></i>
                    </div>
                    <div class="exam-file-info">
                        <div class="exam-file-name">${file.name}</div>
                        <div class="exam-file-size">${this.formatFileSize(file.size)}</div>
                    </div>
                    <button class="exam-file-remove" onclick="this.parentNode.remove()">
                        <i class="fas fa-times"></i>
                    </button>
                `;
                preview.appendChild(fileItem);
            });
        }
    }

    formatFileSize(bytes) {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB', 'GB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
    }

    initializeDatePickers() {
        // Initialize date pickers if needed
        document.querySelectorAll('input[type="date"]').forEach(input => {
            // Add custom styling or functionality if needed
        });
    }

    initializeSearch() {
        document.querySelectorAll('.exam-search-input').forEach(input => {
            input.addEventListener('input', (e) => {
                this.performSearch(e.target.value);
            });
        });
    }

    performSearch(query) {
        const searchTarget = document.querySelector('[data-search-target]');
        if (searchTarget) {
            const items = searchTarget.querySelectorAll('[data-searchable]');
            
            items.forEach(item => {
                const text = item.textContent.toLowerCase();
                const matches = text.includes(query.toLowerCase());
                item.style.display = matches ? '' : 'none';
            });
        }
    }

    initializeProgressBars() {
        document.querySelectorAll('.exam-progress-bar').forEach(bar => {
            const percentage = bar.dataset.percentage || 0;
            setTimeout(() => {
                bar.style.width = percentage + '%';
            }, 100);
        });
    }

    /* ========================================
       GLOBAL CLICK HANDLER
       ======================================== */

    handleGlobalClick(e) {
        // Handle any global click events
    }

    /* ========================================
       UTILITY METHODS
       ======================================== */

    debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    throttle(func, limit) {
        let inThrottle;
        return function() {
            const args = arguments;
            const context = this;
            if (!inThrottle) {
                func.apply(context, args);
                inThrottle = true;
                setTimeout(() => inThrottle = false, limit);
            }
        };
    }
}

// Initialize the theme when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    window.examAppTheme = new ExamAppTheme();
});

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ExamAppTheme;
}
