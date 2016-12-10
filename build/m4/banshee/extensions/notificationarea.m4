AC_DEFUN([BANSHEE_CHECK_EXTENSION_NOTIFICATIONAREA],
[
    AC_ARG_ENABLE(notificationarea, AC_HELP_STRING([--enable-notificationarea], [Build NotificationArea]),, enable_notificationarea="yes")
    AM_CONDITIONAL(ENABLE_NOTIFICATIONAREA, test "x$enable_notificationarea" = "xyes")
])
