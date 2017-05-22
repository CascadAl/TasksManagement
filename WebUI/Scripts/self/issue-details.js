var comments = $('.comment-body>p');
var rawData = $('.comment-body>pre');

var simplemde = new SimpleMDE({
    element: document.getElementById("Text"),
    autoDownloadFontAwesome: false,
    spellChecker: false,
    placeholder: 'Leave a comment'
});
var simplemdeEdit = null;

for (var i = 0; i < comments.length; i++) {
    comments[i].innerHTML = marked(rawData[i].innerText);

    if (rawData[i].innerText.length == 0 || rawData[i].innerText === ' ')
        comments[i].innerHTML = '<em>No description provided</em>';
}

$('.remove-comment').on('click', function() {
    var commentBlock = $(this).closest('.row.comment-container');
    var commentId = commentBlock.find('input[type=hidden]').val();

    $.post('/Issues/RemoveComment', { CommentId: commentId })
        .success(function() {
            commentBlock.remove();
        }).error(function() {
            console.log("error deletion");
        });
});

$('.edit-comment').on('click', function() {
    var commentBlock = $(this).closest('.row.comment-container');
    var commentId = commentBlock.find('input[type=hidden]').val();
    var issueId = $('.comment-editor #IssueId').val();
    var groupId = $('.comment-editor #GroupId').val();

    commentBlock.find('.comment-content').addClass('hidden');
    commentBlock.find('.comment-editor').removeClass('hidden');

    simplemdeEdit = new SimpleMDE({
        element: commentBlock.find('textarea')[0],
        autoDownloadFontAwesome: false,
        spellChecker: false,
        placeholder: 'Leave a comment',
        autofocus: true
    });

    simplemdeEdit.value(commentBlock.find('.comment-body>pre.hidden')[0].innerText);

    commentBlock.find('.js-cancel-comment-edit').on('click', function() {
        commentBlock.find('.comment-content').removeClass('hidden');
        commentBlock.find('.comment-editor').addClass('hidden');
        commentBlock.find('.validation-error').addClass('hidden');

        simplemdeEdit.toTextArea();
        simplemdeEdit = null;

        $(this).off();
    });

    commentBlock.find('.js-update-comment').on('click', function() {
        var commentText = simplemdeEdit.value();
        if (commentText.length < 2) {
            commentBlock.find('.validation-error').removeClass('hidden');
            return;
        }
        var button = $(this);

        $.post('/Issues/SaveComment',
            {
                Id: commentId,
                Text: simplemdeEdit.value(),
                GroupId: groupId,
                IssueId: issueId,
                __RequestVerificationToken: $('.comment-editor form>input').first().val()
            })
            .success(function() {
                location.reload();
            }).error(function() {
                console.log("error editing");
            });

    });
});

$(document).on('click', '#closeIssueButton', function() {
    var issueId = $('#IssueId').val();
    var button = this;

    $.post('/Issues/Close', { IssueId: issueId })
        .success(function(data, textStatus, xhr) {
            if (xhr.status === 200)
                toogleIssueState(button);
        }).error(function() {
            console.log("Error: can't open an issue");
        });
});

$(document).on('click', '#openIssueButton', function() {
    var issueId = $('#IssueId').val();
    var button = this;

    $.post('/Issues/Open', { IssueId: issueId })
        .success(function(data, textStatus, xhr) {
            if (xhr.status === 200)
                toogleIssueState(button);
            console.log(xhr.status);
        }).error(function() {
            console.log("Error: can't open an issue");
        });
});

var toogleIssueState = function(button) {
    var titleLabel = $('.issue-title').find('.js-issue-status-label');
    var issueStatus = titleLabel.hasClass('label-success') ? 'open' : 'closed';

    titleLabel.toggleClass('label-success label-danger');
    titleLabel.children().first().toggleClass('glyphicon-exclamation-sign glyphicon-ban-circle');
    titleLabel.find('.js-status-text').text(issueStatus === 'open' ? 'Closed' : 'Open');

    button.innerText = issueStatus === 'open' ? 'Open issue' : 'Close issue';
    button.setAttribute('id', issueStatus === 'open' ? 'openIssueButton' : 'closeIssueButton');

    $('.comment-list .issue-closed').remove();
};