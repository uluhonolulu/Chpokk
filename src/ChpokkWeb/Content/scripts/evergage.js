var _aaq = _aaq || [];
var evergageAccount = 'uluhonolulu';
var dataset = 'engage';
_aaq.push(['setEvergageAccount', evergageAccount], ['setDataset', dataset], ['setUseSiteConfig', true]);

(function(){
    var d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
    g.type = 'text/javascript'; g.defer = true; g.async = true;
    g.src = document.location.protocol + '//cdn.evergage.com/beacon/'
            + evergageAccount + '/' + dataset + '/scripts/evergage.min.js';
    s.parentNode.insertBefore(g, s);
})();