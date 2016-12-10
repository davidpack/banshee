AC_DEFUN([BANSHEE_CHECK_EXTENSION_SOUNDMENU],
[
    AC_ARG_ENABLE(soundmenu, AC_HELP_STRING([--enable-soundmenu], [Build SoundMenu]),, enable_soundmenu="yes")
    AM_CONDITIONAL(ENABLE_SOUNDMENU, test "x$enable_soundmenu" = "xyes")
])
