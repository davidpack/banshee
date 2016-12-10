AC_DEFUN([BANSHEE_CHECK_EXTENSION_MIROGUIDE],
[
    AC_ARG_ENABLE(miroguide, AC_HELP_STRING([--enable-miroguide], [Build MiroGuide]))

    AS_CASE([$enable_miroguide],
    [no],
    [
        has_miroguide=no
    ],
    [yes],
    [
        BANSHEE_CHECK_WEBBROWSER(has_miroguide=yes)
    ],
    [
        BANSHEE_CHECK_WEBBROWSER(has_miroguide=yes, has_miroguide=no)

        enable_miroguide="auto ($has_miroguide)"
    ])

    AM_CONDITIONAL(ENABLE_MIROGUIDE, test "x$has_miroguide" = "xyes")
])
