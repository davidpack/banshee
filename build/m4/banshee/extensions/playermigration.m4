AC_DEFUN([BANSHEE_CHECK_EXTENSION_PLAYERMIGRATION],
[
    AC_ARG_ENABLE(playermigration, AC_HELP_STRING([--enable-playermigration], [Build PlayerMigration]),, enable_playermigration="yes")
    AM_CONDITIONAL(ENABLE_PLAYERMIGRATION, test "x$enable_playermigration" = "xyes")
])
