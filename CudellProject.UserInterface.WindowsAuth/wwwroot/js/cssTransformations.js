function colapseSearch() {
    $('#search_div_col').css({ 'display': 'flex' });
    $('#search_div_ext').css({ 'display': 'none' });
}

function extendSearch() {
    $('#search_div_col').css({ 'display': 'none' });
    $('#search_div_ext').css({ 'display': 'flex' });
}

function messagePage() {
    $('#message_tab_unactive').css({ 'display': 'none' });
    $('#message_tab_active').css({ 'display': 'flex' });
}

function engPage() {
    $('#eng_unactive').css({ 'display': 'none' });
    $('#eng_active').css({ 'display': 'flex' });
}