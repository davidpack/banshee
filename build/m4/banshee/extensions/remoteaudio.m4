AC_DEFUN([BANSHEE_CHECK_EXTENSION_REMOTEAUDIO],
[
    AC_ARG_ENABLE(remoteaudio, AC_HELP_STRING([--enable-remoteaudio], [Build RemoteAudio]))

    AS_CASE([$enable_remoteaudio],
    [no],
    [
        has_remoteaudio=no
    ],
    [yes],
    [
        BANSHEE_HAS_DAAP(has_remoteaudio=yes)
    ],
    [
        BANSHEE_HAS_DAAP(has_remoteaudio=yes, has_remoteaudio=no)

        enable_remoteaudio="auto ($has_remoteaudio)"
    ])

    AM_CONDITIONAL(ENABLE_REMOTEAUDIO, test "x$has_remoteaudio" = "xyes")
])
