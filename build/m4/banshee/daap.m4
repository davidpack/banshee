AC_DEFUN([BANSHEE_HAS_DAAP],
[
    AM_COND_IF(WITH_DAAP, [$1],
    [
        m4_if([$2],, [AC_MSG_ERROR([Requires DAAP])], [$2])
    ])
])

AC_DEFUN([BANSHEE_CHECK_DAAP],
[
    MZC_REQUIRED=0.7.3

    AC_ARG_WITH(daap, AC_HELP_STRING([--with-daap], [With DAAP support]))

    AS_CASE([$with_daap],
    [no],
    [
        has_daap=no
    ],
    [yes],
    [
        PKG_CHECK_MODULES(MONO_ZEROCONF, mono-zeroconf >= $MZC_REQUIRED, has_daap=yes)
    ],
    [
        PKG_CHECK_MODULES(MONO_ZEROCONF, mono-zeroconf >= $MZC_REQUIRED, has_daap=yes, has_daap=no)

        with_daap="auto ($has_daap)"
    ])

    AM_CONDITIONAL(WITH_DAAP, test "x$has_daap" = "xyes")
])
