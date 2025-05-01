// Updates Payment Esitmator form on Home/Details when information is entered

function PaymentEstimator() {
    // Grab elements
    var card = document.getElementById('paymentEstimator');
    if (!card) return;
    var price = parseFloat(card.dataset.price) || 0;
    var daysInput = document.getElementById('days');
    var ageSelect = document.getElementById('ageBracket');
    var estimateEl = document.getElementById('estimate');
    var calculateBtn = document.getElementById('calculateBtn');

    if (!calculateBtn) return;

    calculateBtn.addEventListener('click', function () {
        var days = parseFloat(daysInput.value) || 0;
        var multiplier = parseFloat(ageSelect.value) || 0;
        var total = price * days * multiplier;
        estimateEl.textContent = total.toLocaleString('en-US', {
            style: 'currency',
            currency: 'USD'
        });
    });
}

// Initialize
document.addEventListener('DOMContentLoaded', PaymentEstimator);
