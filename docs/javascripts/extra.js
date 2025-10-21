// Custom JavaScript for Automation System Documentation

document.addEventListener('DOMContentLoaded', function() {
  console.log('Documentation Automation System loaded');
  
  // Add copy button feedback
  document.querySelectorAll('.md-clipboard').forEach(button => {
    button.addEventListener('click', function() {
      const originalText = this.textContent;
      this.textContent = '✓ Copié !';
      setTimeout(() => {
        this.textContent = originalText;
      }, 2000);
    });
  });
});

