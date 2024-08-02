window.onload = function(event) {
    var el = document.getElementById("content-page");
    var el_height = el.offsetHeight;
    var prec = 0.5*el_height + 50;
    if (prec<100)
    {
        prec = 100;
    }
    el.style.top='max('+prec+'px,50%)';
};
window.onresize = function(event) {
    var el = document.getElementById("content-page");
    var el_height = el.offsetHeight;
    var prec = 0.5*el_height + 50;
    if (prec<100)
    {
        prec = 100;
    }
    el.style.top='max('+prec+'px,50%)';
};
