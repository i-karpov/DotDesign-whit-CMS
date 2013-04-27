(function ($) {
    $.fn.setInputPlaceholder = function (placeholder) {
        this.each(function () {
            var $this = $(this);
            $this.focus(function () {
                if ($this.val() === placeholder) {
                    $this.val("");
                }
            });
            $this.blur(function () {
                if ($this.val() === "") {
                    $this.val(placeholder);
                }
            });
        });
    };
})($);