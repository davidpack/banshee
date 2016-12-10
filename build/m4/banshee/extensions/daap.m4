AC_DEFUN([BANSHEE_CHECK_EXTENSION_DAAP],
[
    AC_ARG_ENABLE(daap, AC_HELP_STRING([--enable-daap], [Enable DAAP support]))

    AS_CASE([$enable_daap],
    [no],
    [
        has_daap=no
    ],
    [yes],
    [
        BANSHEE_HAS_DAAP(has_daap=yes)
    ],
    [
        BANSHEE_HAS_DAAP(has_daap=yes,has_daap=no)

        enable_daap="auto ($has_daap)"
    ])

    AM_CONDITIONAL(ENABLE_DAAP, test "x$has_daap" = "xyes")
])
