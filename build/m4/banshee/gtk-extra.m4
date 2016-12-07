AC_DEFUN([BANSHEE_CHECK_GTK_EXTRA],
[
    LIBGTK_SO_MAP=$(basename $(find $($PKG_CONFIG --variable=libdir gtk+-3.0) -maxdepth 1 -regex '.*libgtk-3\.so\.[[0-9]][[0-9]]*$' | sort | tail -n 1))
    AC_SUBST(LIBGTK_SO_MAP)
])

