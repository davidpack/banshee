AC_DEFUN([BANSHEE_CHECK_EXTENSION_WIKIPEDIA],
[
    AC_ARG_ENABLE(wikipedia, AC_HELP_STRING([--enable-wikipedia], [Build Wikipedia]))

    AS_CASE([$enable_wikipedia],
    [no],
    [
        has_wikipedia=no
    ],
    [yes],
    [
        BANSHEE_CHECK_WEBBROWSER(has_wikipedia=yes)
    ],
    [
        BANSHEE_CHECK_WEBBROWSER(has_wikipedia=yes, has_wikipedia=no)

        enable_wikipedia="auto ($has_wikipedia)"
    ])

    AM_CONDITIONAL(ENABLE_WIKIPEDIA, test "x$has_wikipedia" = "xyes")
])
