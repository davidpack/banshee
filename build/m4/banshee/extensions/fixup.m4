AC_DEFUN([BANSHEE_CHECK_EXTENSION_FIXUP],
[
    AC_ARG_ENABLE(fixup, AC_HELP_STRING([--enable-fixup], [Build Fixup]),, enable_fixup="yes")
    AM_CONDITIONAL(ENABLE_FIXUP, test "x$enable_fixup" = "xyes")
])
