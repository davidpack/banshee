AC_DEFUN([BANSHEE_CHECK_CLIENT_MEDIAPANEL],
[
    AC_ARG_ENABLE(mediapanel, AC_HELP_STRING([--enable-mediapanel], [Enable Mediapanel Client]),, enable_mediapanel="yes")
    AM_CONDITIONAL([ENABLE_CLIENT_MEDIAPANEL], [test "x$enable_mediapanel" = "xyes"])
])
