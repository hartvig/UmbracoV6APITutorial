//this is to convert hex to rgb if the skin configuration is run
function HexToR(h) { return parseInt((cutHex(h)).substring(0, 2), 16) }
function HexToG(h) { return parseInt((cutHex(h)).substring(2, 4), 16) }
function HexToB(h) { return parseInt((cutHex(h)).substring(4, 6), 16) }
function cutHex(h) { return (h.charAt(0) == "#") ? h.substring(1, 7) : h }



 function includeCSS(p_file) {
        var v_css = document.createElement('link');
        v_css.rel = 'stylesheet'
        v_css.type = 'text/css';
        v_css.href = p_file;
        document.getElementsByTagName('head')[0].appendChild(v_css);
}


$(document).ready(function () {
  
        $(".tagcloud a").hover(function(){
          normal = $(this).css("background");
          $(this).css({
            'padding' : '4px 6px',
            'margin-bottom' : '-3px',
            'margin-top' : '-3px',
            'background' : 'rgba(54, 136, 175, 1)'
          })
        }, function(){
          $(this).css({
            'padding' : '1px 6px',
            'margin' : '4px 1px',
            'background' : normal
          })
        });
          // was using :before psuedo selectors but IE9 decided it didn't want to apply border-radius pseudo elements :(
        $("li.current").append("<span class=\"marker\"></span>");

});