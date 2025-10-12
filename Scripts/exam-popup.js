/**
 * Exam Popup Modal System
 * Reusable popup modal system for alerts, confirms, and custom messages
 * 
 * Usage Examples:
 * 
 * // Simple alert
 * showAlert('This is an alert message');
 * 
 * // Alert with title and type
 * showAlert('Operation completed successfully!', 'Success', 'success');
 * 
 * // Confirmation dialog
 * showConfirm('Are you sure you want to delete this item?', 'Confirm Delete', 
 *     function() { console.log('User confirmed'); },
 *     function() { console.log('User cancelled'); }
 * );
 * 
 * // Convenience functions
 * showSuccess('Data saved successfully!');
 * showError('Something went wrong!');
 * showWarning('Please check your input!');
 * showInfo('Here is some information!');
 * 
 * // Custom popup with multiple buttons
 * examPopup.show({
 *     type: 'question',
 *     title: 'Choose Action',
 *     message: 'What would you like to do?',
 *     buttons: [
 *         { text: 'Save', class: 'exam-popup-btn-success', action: 'save' },
 *         { text: 'Cancel', class: 'exam-popup-btn-secondary', action: 'cancel' }
 *     ]
 * });
 */

class ExamPopup {
    constructor() {
        this.overlay = null;
        this.modal = null;
        this.callbacks = {};
    }

    /**
     * Create and show an alert popup
     * @param {string} message - The message to display
     * @param {string} title - The popup title (optional)
     * @param {string} type - The popup type: 'info', 'success', 'warning', 'error' (optional)
     * @param {Function} callback - Callback function when popup is closed (optional)
     */
    alert(message, title = 'Information', type = 'info', callback = null) {
        const config = {
            type: type,
            title: title,
            message: message,
            buttons: [
                {
                    text: 'OK',
                    class: this.getButtonClass(type),
                    action: 'close'
                }
            ]
        };

        this.show(config, callback);
    }

    /**
     * Create and show a confirm popup
     * @param {string} message - The message to display
     * @param {string} title - The popup title (optional)
     * @param {Function} onConfirm - Callback when user confirms (optional)
     * @param {Function} onCancel - Callback when user cancels (optional)
     */
    confirm(message, title = 'Confirm', onConfirm = null, onCancel = null) {
        const config = {
            type: 'warning',
            title: title,
            message: message,
            buttons: [
                {
                    text: 'Cancel',
                    class: 'exam-popup-btn-secondary',
                    action: 'cancel'
                },
                {
                    text: 'Confirm',
                    class: 'exam-popup-btn-primary',
                    action: 'confirm'
                }
            ]
        };

        this.callbacks = {
            confirm: onConfirm,
            cancel: onCancel
        };

        this.show(config);
    }

    /**
     * Create and show a custom popup
     * @param {Object} config - Configuration object
     * @param {Function} callback - Callback function (optional)
     */
    show(config, callback = null) {
        // Remove existing popup if any
        this.hide();

        // Create popup HTML
        const popupHTML = this.createPopupHTML(config);
        
        // Add to DOM
        document.body.insertAdjacentHTML('beforeend', popupHTML);
        
        // Get references
        this.overlay = document.querySelector('.exam-popup-overlay:last-child');
        this.modal = this.overlay.querySelector('.exam-popup-modal');

        // Show popup
        setTimeout(() => {
            this.overlay.classList.add('show');
        }, 10);

        // Set up event listeners
        this.setupEventListeners();

        // Store callback
        if (callback) {
            this.callbacks.close = callback;
        }
    }

    /**
     * Hide the current popup
     */
    hide() {
        if (this.overlay) {
            this.overlay.classList.add('fade-out');
            setTimeout(() => {
                if (this.overlay && this.overlay.parentNode) {
                    this.overlay.parentNode.removeChild(this.overlay);
                }
                this.overlay = null;
                this.modal = null;
                this.callbacks = {};
            }, 300);
        }
    }

    /**
     * Create popup HTML based on configuration
     * @param {Object} config - Configuration object
     * @returns {string} HTML string
     */
    createPopupHTML(config) {
        const icon = this.getIcon(config.type);
        const headerClass = this.getHeaderClass(config.type);
        
        let buttonsHTML = '';
        if (config.buttons && config.buttons.length > 0) {
            buttonsHTML = config.buttons.map(button => 
                `<button class="exam-popup-btn ${button.class}" data-action="${button.action}">
                    ${button.icon ? `<i class="${button.icon}"></i>` : ''}
                    ${button.text}
                </button>`
            ).join('');
        }

        return `
            <div class="exam-popup-overlay">
                <div class="exam-popup-modal">
                    <div class="exam-popup-header ${headerClass}">
                        <h3 class="exam-popup-title">
                            <i class="exam-popup-icon ${icon}"></i>
                            ${config.title || 'Information'}
                        </h3>
                        <button class="exam-popup-close" data-action="close">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                    <div class="exam-popup-body">
                        <p class="exam-popup-message">${config.message}</p>
                        ${config.details ? `<div class="exam-popup-details">${config.details}</div>` : ''}
                    </div>
                    ${buttonsHTML ? `<div class="exam-popup-footer">${buttonsHTML}</div>` : ''}
                </div>
            </div>
        `;
    }

    /**
     * Set up event listeners for the popup
     */
    setupEventListeners() {
        if (!this.overlay) return;

        // Close button
        const closeBtn = this.overlay.querySelector('.exam-popup-close');
        if (closeBtn) {
            closeBtn.addEventListener('click', () => this.handleAction('close'));
        }

        // Action buttons
        const actionButtons = this.overlay.querySelectorAll('[data-action]');
        actionButtons.forEach(button => {
            button.addEventListener('click', (e) => {
                const action = e.currentTarget.getAttribute('data-action');
                this.handleAction(action);
            });
        });

        // Overlay click to close
        this.overlay.addEventListener('click', (e) => {
            if (e.target === this.overlay) {
                this.handleAction('close');
            }
        });

        // Escape key to close
        const escapeHandler = (e) => {
            if (e.key === 'Escape') {
                this.handleAction('close');
                document.removeEventListener('keydown', escapeHandler);
            }
        };
        document.addEventListener('keydown', escapeHandler);
    }

    /**
     * Handle button actions
     * @param {string} action - The action to perform
     */
    handleAction(action) {
        switch (action) {
            case 'close':
                this.hide();
                if (this.callbacks.close) {
                    this.callbacks.close();
                }
                break;
            case 'confirm':
                this.hide();
                if (this.callbacks.confirm) {
                    this.callbacks.confirm();
                }
                break;
            case 'cancel':
                this.hide();
                if (this.callbacks.cancel) {
                    this.callbacks.cancel();
                }
                break;
        }
    }

    /**
     * Get icon class based on popup type
     * @param {string} type - The popup type
     * @returns {string} Icon class
     */
    getIcon(type) {
        const icons = {
            info: 'fas fa-info-circle',
            success: 'fas fa-check-circle',
            warning: 'fas fa-exclamation-triangle',
            error: 'fas fa-times-circle',
            question: 'fas fa-question-circle'
        };
        return icons[type] || icons.info;
    }

    /**
     * Get button class based on popup type
     * @param {string} type - The popup type
     * @returns {string} Button class
     */
    getButtonClass(type) {
        const classes = {
            info: 'exam-popup-btn-primary',
            success: 'exam-popup-btn-success',
            warning: 'exam-popup-btn-warning',
            error: 'exam-popup-btn-danger'
        };
        return classes[type] || 'exam-popup-btn-primary';
    }

    /**
     * Get header class based on popup type
     * @param {string} type - The popup type
     * @returns {string} Header class
     */
    getHeaderClass(type) {
        const classes = {
            info: '',
            success: 'exam-popup-header-success',
            warning: 'exam-popup-header-warning',
            error: 'exam-popup-header-error'
        };
        return classes[type] || '';
    }
}

// Create global instance
window.examPopup = new ExamPopup();

// Convenience functions
window.showAlert = function(message, title, type, callback) {
    return window.examPopup.alert(message, title, type, callback);
};

window.showConfirm = function(message, title, onConfirm, onCancel) {
    return window.examPopup.confirm(message, title, onConfirm, onCancel);
};

window.showSuccess = function(message, title = 'Success', callback) {
    return window.examPopup.alert(message, title, 'success', callback);
};

window.showError = function(message, title = 'Error', callback) {
    return window.examPopup.alert(message, title, 'error', callback);
};

window.showWarning = function(message, title = 'Warning', callback) {
    return window.examPopup.alert(message, title, 'warning', callback);
};

window.showInfo = function(message, title = 'Information', callback) {
    return window.examPopup.alert(message, title, 'info', callback);
};

// Add header color variants for different popup types
const style = document.createElement('style');
style.textContent = `
    .exam-popup-header-success {
        background: linear-gradient(135deg, var(--green-500) 0%, var(--green-600) 100%) !important;
    }
    
    .exam-popup-header-warning {
        background: linear-gradient(135deg, var(--yellow-500) 0%, var(--yellow-600) 100%) !important;
    }
    
    .exam-popup-header-error {
        background: linear-gradient(135deg, var(--red-500) 0%, var(--red-600) 100%) !important;
    }
`;
document.head.appendChild(style);
