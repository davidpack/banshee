AC_DEFUN([BANSHEE_CHECK_CLIENT_MUINSHEE],
[
    AC_ARG_ENABLE(muinshee, AC_HELP_STRING([--enable-muinshee], [Enable Muinshee Client]),, enable_muinshee="yes")
    AM_CONDITIONAL([ENABLE_CLIENT_MUINSHEE], [test "x$enable_muinshee" = "xyes"])
])

