AC_DEFUN([BANSHEE_CHECK_EXTENSION_EMUSIC_STORE],
[
    AC_ARG_ENABLE(emusic-store, AC_HELP_STRING([--enable-emusic-store], [Build Emusic.Store]))

    AS_CASE([$enable_emusic_store],
    [no],
    [
        has_emusic_store=no
    ],
    [yes],
    [
        BANSHEE_CHECK_WEBBROWSER(has_emusic_store=yes)
    ],
    [
        BANSHEE_CHECK_WEBBROWSER(has_emusic_store=yes, has_emusic_store=no)

        enable_emusic_store="auto ($has_emusic_store)"
    ])

    AM_CONDITIONAL(ENABLE_EMUSIC_STORE, test "x$has_emusic_store" = "xyes")
])
