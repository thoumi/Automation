/* ===============================================
   AUTOMATION SYSTEM - PROFESSIONAL DOCUMENTATION
   Custom JavaScript Enhancements
   =============================================== */

document.addEventListener("DOMContentLoaded", function() {
    
    // === SMOOTH SCROLLING ===
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // === BACK TO TOP BUTTON ===
    const backToTopButton = document.createElement("button");
    backToTopButton.innerHTML = `
        <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M7 14l5-5 5 5"/>
        </svg>
    `;
    backToTopButton.className = "back-to-top";
    backToTopButton.setAttribute("aria-label", "Retour en haut");
    backToTopButton.style.cssText = `
        position: fixed;
        bottom: 2rem;
        right: 2rem;
        width: 3rem;
        height: 3rem;
        background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
        color: white;
        border: none;
        border-radius: 50%;
        cursor: pointer;
        box-shadow: 0 4px 15px rgba(99, 102, 241, 0.3);
        transition: all 0.3s ease;
        opacity: 0;
        visibility: hidden;
        z-index: 1000;
        display: flex;
        align-items: center;
        justify-content: center;
    `;
    
    document.body.appendChild(backToTopButton);

    // Show/hide back to top button
    window.addEventListener("scroll", function() {
        if (window.scrollY > 300) {
            backToTopButton.style.opacity = "1";
            backToTopButton.style.visibility = "visible";
        } else {
            backToTopButton.style.opacity = "0";
            backToTopButton.style.visibility = "hidden";
        }
    });

    // Back to top functionality
    backToTopButton.addEventListener("click", function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // Hover effects
    backToTopButton.addEventListener("mouseenter", function() {
        this.style.transform = "translateY(-2px)";
        this.style.boxShadow = "0 8px 25px rgba(99, 102, 241, 0.4)";
    });

    backToTopButton.addEventListener("mouseleave", function() {
        this.style.transform = "translateY(0)";
        this.style.boxShadow = "0 4px 15px rgba(99, 102, 241, 0.3)";
    });

    // === ANIMATION ON SCROLL ===
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    // Observe all cards and sections
    document.querySelectorAll('.tech-card, .stat-card, .feature-card').forEach(card => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        card.style.transition = 'all 0.6s ease';
        observer.observe(card);
    });

    // === ENHANCED CODE BLOCKS ===
    document.querySelectorAll('pre code').forEach(block => {
        // Add copy button
        const copyButton = document.createElement('button');
        copyButton.innerHTML = '📋';
        copyButton.className = 'copy-code-btn';
        copyButton.style.cssText = `
            position: absolute;
            top: 0.5rem;
            right: 0.5rem;
            background: rgba(99, 102, 241, 0.1);
            border: 1px solid rgba(99, 102, 241, 0.3);
            border-radius: 4px;
            padding: 0.25rem 0.5rem;
            font-size: 0.8rem;
            cursor: pointer;
            transition: all 0.2s ease;
        `;
        
        copyButton.addEventListener('click', function() {
            navigator.clipboard.writeText(block.textContent).then(() => {
                this.innerHTML = '✅';
                setTimeout(() => {
                    this.innerHTML = '📋';
                }, 2000);
            });
        });

        const pre = block.parentElement;
        pre.style.position = 'relative';
        pre.appendChild(copyButton);
    });

    // === ENHANCED NAVIGATION ===
    const navLinks = document.querySelectorAll('.md-nav__link');
    navLinks.forEach(link => {
        link.addEventListener('click', function() {
            // Add active state
            navLinks.forEach(l => l.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // === PERFORMANCE MONITORING ===
    if ('performance' in window) {
        window.addEventListener('load', function() {
            const perfData = performance.getEntriesByType('navigation')[0];
            console.log(`🚀 Automation System Documentation loaded in ${Math.round(perfData.loadEventEnd - perfData.loadEventStart)}ms`);
        });
    }

    // === CONSOLE BRANDING ===
    console.log(`
    🚀 AUTOMATION SYSTEM DOCUMENTATION
    =================================
    Version: 1.0.0
    Author: Thoumi
    Repository: https://github.com/thoumi/Automation
    Documentation: https://thoumi.github.io/Automation/
    
    Built with ❤️ for professional automation
    `);

});

// === UTILITY FUNCTIONS ===
window.AutomationSystem = {
    version: '1.0.0',
    author: 'Thoumi',
    repository: 'https://github.com/thoumi/Automation',
    
    // Utility function to highlight code
    highlightCode: function(selector) {
        document.querySelectorAll(selector).forEach(block => {
            block.style.background = 'linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%)';
            block.style.border = '1px solid #e2e8f0';
            block.style.borderRadius = '8px';
            block.style.padding = '1rem';
        });
    },
    
    // Utility function to animate elements
    animate: function(selector, animation = 'fadeInUp') {
        document.querySelectorAll(selector).forEach(el => {
            el.style.animation = `${animation} 0.6s ease forwards`;
        });
    }
};