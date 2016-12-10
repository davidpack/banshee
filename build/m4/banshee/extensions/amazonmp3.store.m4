AC_DEFUN([BANSHEE_CHECK_EXTENSION_AMAZONMP3_STORE],
[
    AC_ARG_ENABLE(amazonmp3-store, AC_HELP_STRING([--enable-amazonmp3-store], [Build AmazonMp3.Store]))

    AS_CASE([$enable_amazonmp3_store],
    [no],
    [
        has_amazonmp3_store=no
    ],
    [yes],
    [
        BANSHEE_CHECK_WEBBROWSER(has_amazonmp3_store=yes)
    ],
    [
        BANSHEE_CHECK_WEBBROWSER(has_amazonmp3_store=yes, has_amazonmp3_store=no)

        enable_amazonmp3_store="auto ($has_amazonmp3_store)"
    ])

    AM_CONDITIONAL(ENABLE_AMAZONMP3_STORE, test "x$has_amazonmp3_store" = "xyes")
])
