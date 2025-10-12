/* ========================================
   MFANO THEME JAVASCRIPT
   Interactive functionality for MFANO-inspired theme
   ======================================== */

$(document).ready(function() {
    'use strict';

    // Initialize MFANO Theme
    initializeMfanoTheme();

    // Form validation
    initializeFormValidation();

    // Button interactions
    initializeButtonInteractions();

    // Sidebar toggle for mobile
    initializeSidebarToggle();

    // Auto-resize textareas
    initializeAutoResize();

    // Input focus effects
    initializeInputFocus();

    // File upload preview
    initializeFileUpload();
});

/* ========================================
   CORE FUNCTIONS
   ======================================== */

function initializeMfanoTheme() {
    // Add loading animation to buttons
    $('.app-btn').on('click', function() {
        var $btn = $(this);
        if (!$btn.hasClass('loading')) {
            $btn.addClass('loading');
            
            // Simulate loading for demo purposes
            setTimeout(function() {
                $btn.removeClass('loading');
            }, 2000);
        }
    });

    // Add ripple effect to buttons
    $('.app-btn').on('click', function(e) {
        var $btn = $(this);
        var ripple = $('<span class="ripple"></span>');
        
        var rect = this.getBoundingClientRect();
        var size = Math.max(rect.width, rect.height);
        var x = e.clientX - rect.left - size / 2;
        var y = e.clientY - rect.top - size / 2;
        
        ripple.css({
            width: size,
            height: size,
            left: x,
            top: y
        });
        
        $btn.append(ripple);
        
        setTimeout(function() {
            ripple.remove();
        }, 600);
    });
}

function initializeFormValidation() {
    // Real-time validation
    $('.app-form-control, .app-form-select, .app-form-textarea').on('blur', function() {
        validateField($(this));
    });

    // Form submission validation
    $('form').on('submit', function(e) {
        var isValid = true;
        var $form = $(this);
        
        // Validate all required fields
        $form.find('[required]').each(function() {
            if (!validateField($(this))) {
                isValid = false;
            }
        });
        
        if (!isValid) {
            e.preventDefault();
            showNotification('Please fill in all required fields correctly.', 'error');
        }
    });
}

function validateField($field) {
    var value = $field.val().trim();
    var isValid = true;
    var message = '';
    
    // Remove existing validation message
    $field.siblings('.app-validation-message').remove();
    
    if ($field.prop('required') && !value) {
        isValid = false;
        message = 'This field is required.';
    } else if (value) {
        // Email validation
        if ($field.attr('type') === 'email') {
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                isValid = false;
                message = 'Please enter a valid email address.';
            }
        }
        
        // Phone validation
        if ($field.attr('type') === 'tel') {
            var phoneRegex = /^[\+]?[1-9][\d]{0,15}$/;
            if (!phoneRegex.test(value.replace(/[\s\-\(\)]/g, ''))) {
                isValid = false;
                message = 'Please enter a valid phone number.';
            }
        }
        
        // Min length validation
        var minLength = $field.attr('minlength');
        if (minLength && value.length < parseInt(minLength)) {
            isValid = false;
            message = 'Minimum length is ' + minLength + ' characters.';
        }
        
        // Max length validation
        var maxLength = $field.attr('maxlength');
        if (maxLength && value.length > parseInt(maxLength)) {
            isValid = false;
            message = 'Maximum length is ' + maxLength + ' characters.';
        }
    }
    
    // Add validation message
    if (message) {
        var $message = $('<div class="app-validation-message ' + (isValid ? 'info' : 'error') + '">' + message + '</div>');
        $field.after($message);
    }
    
    // Update field styling
    $field.removeClass('valid invalid');
    if (value) {
        $field.addClass(isValid ? 'valid' : 'invalid');
    }
    
    return isValid;
}

function initializeButtonInteractions() {
    // Loading state for submit buttons
    $('button[type="submit"], input[type="submit"]').on('click', function() {
        var $btn = $(this);
        var originalText = $btn.text() || $btn.val();
        
        $btn.prop('disabled', true)
            .data('original-text', originalText)
            .html('<span class="spinner"></span> Processing...');
        
        // Re-enable after 3 seconds (in case of errors)
        setTimeout(function() {
            $btn.prop('disabled', false)
                .text(originalText);
        }, 3000);
    });
    
    // Confirmation for delete/cancel actions
    $('.app-btn-danger, .btn-cancel').on('click', function(e) {
        var $btn = $(this);
        var action = $btn.hasClass('app-btn-danger') ? 'delete' : 'cancel';
        
        if (!confirm('Are you sure you want to ' + action + '?')) {
            e.preventDefault();
        }
    });
}

function initializeSidebarToggle() {
    $('.toggle-btn').on('click', function() {
        $('.app-sidebar').toggleClass('open');
        $('.side-overlay').toggleClass('show');
    });
    
    $('.side-overlay').on('click', function() {
        $('.app-sidebar').removeClass('open');
        $('.side-overlay').removeClass('show');
    });
    
    // Close sidebar on window resize if mobile
    $(window).on('resize', function() {
        if ($(window).width() > 768) {
            $('.app-sidebar').removeClass('open');
            $('.side-overlay').removeClass('show');
        }
    });
}

function initializeAutoResize() {
    $('.app-form-textarea').each(function() {
        var $textarea = $(this);
        $textarea.on('input', function() {
            this.style.height = 'auto';
            this.style.height = (this.scrollHeight) + 'px';
        });
        
        // Initial resize
        $textarea.trigger('input');
    });
}

function initializeInputFocus() {
    $('.app-form-control, .app-form-select, .app-form-textarea').on('focus', function() {
        $(this).parent().addClass('focused');
    }).on('blur', function() {
        $(this).parent().removeClass('focused');
    });
}

function initializeFileUpload() {
    $('input[type="file"]').on('change', function() {
        var $input = $(this);
        var $preview = $input.siblings('.file-preview');
        
        if (this.files && this.files[0]) {
            var file = this.files[0];
            var reader = new FileReader();
            
            reader.onload = function(e) {
                if ($preview.length) {
                    if (file.type.startsWith('image/')) {
                        $preview.html('<img src="' + e.target.result + '" alt="Preview" style="max-width: 200px; max-height: 200px;">');
                    } else {
                        $preview.html('<div class="file-info"><i class="ph ph-file"></i> ' + file.name + '</div>');
                    }
                }
            };
            
            reader.readAsDataURL(file);
        }
    });
}

/* ========================================
   UTILITY FUNCTIONS
   ======================================== */

function showNotification(message, type, duration) {
    type = type || 'info';
    duration = duration || 5000;
    
    var $notification = $('<div class="app-notification ' + type + '">' + message + '</div>');
    
    $('body').append($notification);
    
    setTimeout(function() {
        $notification.addClass('show');
    }, 100);
    
    setTimeout(function() {
        $notification.removeClass('show');
        setTimeout(function() {
            $notification.remove();
        }, 300);
    }, duration);
}

function showLoading(element) {
    var $element = $(element);
    $element.addClass('loading');
    
    return function() {
        $element.removeClass('loading');
    };
}

function formatPhoneNumber(input) {
    var value = input.value.replace(/\D/g, '');
    var formattedValue = '';
    
    if (value.length > 0) {
        if (value.length <= 3) {
            formattedValue = value;
        } else if (value.length <= 6) {
            formattedValue = value.slice(0, 3) + '-' + value.slice(3);
        } else {
            formattedValue = value.slice(0, 3) + '-' + value.slice(3, 6) + '-' + value.slice(6, 10);
        }
    }
    
    input.value = formattedValue;
}

function formatCurrency(input) {
    var value = input.value.replace(/[^\d]/g, '');
    var formattedValue = '';
    
    if (value.length > 0) {
        var number = parseInt(value);
        formattedValue = number.toLocaleString();
    }
    
    input.value = formattedValue;
}

/* ========================================
   TAB FUNCTIONALITY
   ======================================== */

function initializeTabs() {
    $('.app-tab-nav').on('click', 'a', function(e) {
        e.preventDefault();
        
        var $tab = $(this);
        var target = $tab.attr('href');
        
        // Update active tab
        $('.app-tab-nav a').removeClass('active');
        $tab.addClass('active');
        
        // Show target content
        $('.app-tab-content').removeClass('active');
        $(target).addClass('active');
    });
}

/* ========================================
   MODAL FUNCTIONALITY
   ======================================== */

function initializeModals() {
    $('.app-modal-trigger').on('click', function(e) {
        e.preventDefault();
        
        var target = $(this).attr('href');
        var $modal = $(target);
        
        $modal.addClass('show');
        $('body').addClass('modal-open');
    });
    
    $('.app-modal-close, .app-modal-overlay').on('click', function() {
        $('.app-modal').removeClass('show');
        $('body').removeClass('modal-open');
    });
    
    $(document).on('keydown', function(e) {
        if (e.keyCode === 27) { // Escape key
            $('.app-modal').removeClass('show');
            $('body').removeClass('modal-open');
        }
    });
}

/* ========================================
   SEARCH FUNCTIONALITY
   ======================================== */

function initializeSearch() {
    $('.app-search-input').on('input', function() {
        var query = $(this).val().toLowerCase();
        var $items = $('.app-searchable-item');
        
        $items.each(function() {
            var text = $(this).text().toLowerCase();
            if (text.includes(query)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
}

/* ========================================
   ACCORDION FUNCTIONALITY
   ======================================== */

function initializeAccordion() {
    $('.app-accordion-header').on('click', function() {
        var $header = $(this);
        var $content = $header.next('.app-accordion-content');
        var $accordion = $header.parent('.app-accordion-item');
        
        if ($accordion.hasClass('active')) {
            $accordion.removeClass('active');
            $content.slideUp(300);
        } else {
            $('.app-accordion-item').removeClass('active');
            $('.app-accordion-content').slideUp(300);
            
            $accordion.addClass('active');
            $content.slideDown(300);
        }
    });
}

/* ========================================
   DROPDOWN FUNCTIONALITY
   ======================================== */

function initializeDropdowns() {
    $('.app-dropdown-trigger').on('click', function(e) {
        e.preventDefault();
        e.stopPropagation();
        
        var $dropdown = $(this).siblings('.app-dropdown-menu');
        
        $('.app-dropdown-menu').not($dropdown).removeClass('show');
        $dropdown.toggleClass('show');
    });
    
    $(document).on('click', function() {
        $('.app-dropdown-menu').removeClass('show');
    });
}

/* ========================================
   TOOLTIP FUNCTIONALITY
   ======================================== */

function initializeTooltips() {
    $('[data-tooltip]').on('mouseenter', function() {
        var $element = $(this);
        var tooltipText = $element.data('tooltip');
        
        var $tooltip = $('<div class="app-tooltip">' + tooltipText + '</div>');
        $('body').append($tooltip);
        
        var rect = this.getBoundingClientRect();
        $tooltip.css({
            top: rect.top - $tooltip.outerHeight() - 5,
            left: rect.left + (rect.width / 2) - ($tooltip.outerWidth() / 2)
        });
        
        $tooltip.addClass('show');
    }).on('mouseleave', function() {
        $('.app-tooltip').remove();
    });
}

/* ========================================
   INITIALIZE ALL COMPONENTS
   ======================================== */

$(document).ready(function() {
    initializeTabs();
    initializeModals();
    initializeSearch();
    initializeAccordion();
    initializeDropdowns();
    initializeTooltips();
});

/* ========================================
   CSS ANIMATIONS (added via JavaScript)
   ======================================== */

// Add dynamic CSS for animations
var style = document.createElement('style');
style.textContent = `
    .ripple {
        position: absolute;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.6);
        transform: scale(0);
        animation: ripple-animation 0.6s linear;
        pointer-events: none;
    }
    
    @keyframes ripple-animation {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    .spinner {
        display: inline-block;
        width: 16px;
        height: 16px;
        border: 2px solid transparent;
        border-top: 2px solid currentColor;
        border-radius: 50%;
        animation: spin 1s linear infinite;
        margin-right: 8px;
    }
    
    @keyframes spin {
        0% { transform: rotate(0deg); }
        100% { transform: rotate(360deg); }
    }
    
    .app-notification {
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 12px 20px;
        border-radius: 6px;
        color: white;
        font-weight: 500;
        z-index: 10000;
        transform: translateX(400px);
        transition: transform 0.3s ease;
        max-width: 300px;
    }
    
    .app-notification.show {
        transform: translateX(0);
    }
    
    .app-notification.success {
        background: #28a745;
    }
    
    .app-notification.error {
        background: #dc3545;
    }
    
    .app-notification.warning {
        background: #ffc107;
        color: #212529;
    }
    
    .app-notification.info {
        background: #17a2b8;
    }
`;
document.head.appendChild(style);
