setModalDelete = function (url, addIdToQuery) {
    addIdToQuery = typeof addIdToQuery !== 'undefined' ? addIdToQuery : true;
    $('div.modal-background').click(function () {
        $('div#modal-delete').empty();
        $('div.modal-part').hide();
    });
    $('a.delete').click(function (e) {
        $('div.modal-part').show();
        var completeUrl = url;
        if (addIdToQuery) {
            completeUrl += '?' + $.param({
                           id: $(this).data('id')
            });
        }
        $('div#modal-delete').load(completeUrl);
        e.preventDefault();
        return false;
    });
};

