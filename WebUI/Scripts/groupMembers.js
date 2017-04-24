﻿$.validator.setDefaults({ ignore: '' });
$('#UserId').trigger("change");

$(document).ready(function () {
    var users = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('FullName'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/Group/GetUsers/?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#add-user').typeahead({
        minLength: 2,
        highlight: true
    },
        {
            name: 'users',
            display: 'FullName',
            source: users
        }).on('typeahead:select', function (e, user) {
            $('#UserId').val(user.UserId).trigger("change");
            $('#UserId').valid();
        });

    $(document).on('click', '.remove-member', function () {
        var sender = $(this);
        var userToRemoveId = sender.closest('.controls').find('.user-id-js').val();
        var groupId = $('#GroupId').val();

        $.ajax({
            type: 'POST',
            url: '/Group/RemoveMember',
            data: {
                UserToRemove: userToRemoveId,
                GroupId: groupId
            }
        }).done(function (data, textStatus, jqXHR) {
            location.reload();
        });
    });

});