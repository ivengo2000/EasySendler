﻿function TinyMCE() {
    "use strict";
    // Initialize your tinyMCE Editor with your preferred options

    tinyMCE.init({
       
        plugins: "code",
        // General options
        mode : "specific_textareas",
        editor_selector: "mceEditor",
        theme: "modern",
      
      
         // Theme options
        theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_text_colors: "FF00FF,FFFF00,00FF00,FF0000,0000FF,000000",
        theme_advanced_resizing: true,
        branding: false,
        min_height: 400,
        max_height: 800,
        // Example content CSS (should be your site CSS) 
        cssFiles: [
         'theme/neat.css',
         'theme/elegant.css'
        ]
      
    });

}
