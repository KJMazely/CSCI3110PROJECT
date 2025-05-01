// When Contact Us form on home/details is entered and submit is clicked, 
// sends form to businessInquiry.cs and resets form.
document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('buyForm');
    var heading = document.getElementById('contactHeading');
    if (!form || !heading) return;

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        form.reset();
        heading.textContent = "Thank you! We will contact you soon!";
        heading.style.color = "green";
    });
});
