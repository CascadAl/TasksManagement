var users = [];
$(document).ready(function () {

    var simplemde = new SimpleMDE({
        element: document.getElementById("Text"),
        autoDownloadFontAwesome: false,
        spellChecker: false,
        placeholder: 'Leave a comment'
    });

    $('.selectpicker').on('shown.bs.select', function () {
        if (users.length > 0) return;

        getUsers();
    });

    $('.js-delete-issue').confirmation({
        rootSelector: '.js-delete-issue',
        container: 'body'
    });

    $('.js-assign-to-me').on('click', function (e) {
        e.preventDefault();

        var userId = $('#UserId').val();
        var currentUser;

        if (users.length == 0) {
            var promise = getUsers();

            promise.then(function () {
                currentUser = users.filter(function (user) {
                    return user.UserId == userId;
                });

                $('.selectpicker').selectpicker('val', userId);
            });
        }
        else {
            var currentUser = users.filter(function (user) {
                return user.UserId == userId;
            });

            $('.selectpicker').selectpicker('val', userId);
        }
    });

    $('.js-delete-issue').on('click', function (e) {
        e.preventDefault();

        var issueId = $('#Id').val();
        var groupId = $('#GroupId').val();

        $.post('/Issues/Remove?groupId=' + groupId, { issueId: issueId })
            .success(function () {
                window.location = '/Issues/?groupId=' + groupId;
            })
            .error(function () {
                console.log("Error: can't delete an issue");
            });
    });

    var getUsers = function () {
        var deferred = $.Deferred();
        var selectTemplate = '<option value="" selected>Unassigned</option> <option data-divider="true"></option>';
        var groupId = $('#GroupId').val();

        $.get('/Group/Members/' + groupId, function (data) {
            users = data.Members;
            var html = [];

            users.forEach(function (user, i, users) {
                html.push('<option title="' + user.FullName + '" value="' + user.UserId + '" data-content="<span >' + user.FullName + '<br/> @' + user.Username + '</span>"></option>');
            });

            $('.selectpicker').html(selectTemplate + html.join(''));
            $('.selectpicker').selectpicker('refresh');

            deferred.resolve();
        });
        return deferred.promise();
    };

    var initAssignee = function () {
        var assigneeInitialId = $('.selectpicker').data('init');

        if (assigneeInitialId > 0) {
            var promise = getUsers();

            promise.then(function () {
                asignee = users.filter(function (user) {
                    return user.UserId == assigneeInitialId;
                });

                $('.selectpicker').selectpicker('val', assigneeInitialId);
            });
        }
    };

    initAssignee();

    if (!$('#Id').val())
        $('.js-delete-issue').addClass('hidden');

});
