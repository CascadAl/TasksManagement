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
        minLength: 3,
        highlight: true
    },
        {
            name: 'users',
            display: 'FullName',// + '@' + 'Username',
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
        })
            .done(function (data, textStatus, xhr) {
                if (xhr.status === 200) {
                    if (data == userToRemoveId)
                        window.location='/Group';
                    else
                        location.reload();
                }
                else
                    $('#error-alert').show();
            })
            .fail(function () {
                console.log("Error: can't exclude user grom a group")
            });
    });

    $(document).on('change', '.change-group-role-dd', function (e) {
        var userId = $(this).closest('.controls').find('.user-id-js').val();
        var groupId = $('#GroupId').val();
        var roleId = e.target.value;

        $.ajax({
            type: 'POST',
            url: '/Group/ChangeMemberRole',
            data: {
                UserId: userId,
                GroupId: groupId,
                RoleId: roleId
            }
        }).done(function () {
            location.reload();
        });
    });

    $('#add-member-form').on('submit', function (e) {
        var selectedUserId = $('#UserId').val();
        var usersInGroup = $('.group-list').find('.user-id-js');

        var filter = usersInGroup.filter(function (index) {
            return this.value == selectedUserId;
        });

        if (filter.length > 0) {
            e.preventDefault();
            $('#error-alert-userAlreadyHere').show();
        }
    });

    $('.remove-member').confirmation({
        rootSelector: '.remove-member',
        container: 'body'
    });
});
