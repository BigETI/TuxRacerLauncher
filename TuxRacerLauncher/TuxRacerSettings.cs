using System;
using System.Collections.Generic;
using System.IO;

namespace TuxRacerLauncher
{
    public class TuxRacerSettings
    {
        private Dictionary<string, TuxRacerSettingsItem> items = new Dictionary<string, TuxRacerSettingsItem>();

        public TuxRacerSettings()
        {
            //
        }

        public TuxRacerSettingsItem add<T>(string key, T value, bool create_new_if_exists = true)
        {
            TuxRacerSettingsItem ret = null;
            if (items.ContainsKey(key))
            {
                if (create_new_if_exists)
                    ret = new TuxRacerSettingsItem(key, new TuxRacerSettingsItemValue<T>(value));
                else
                {
                    ret = items[key];
                    ret.Value = new TuxRacerSettingsItemValue<T>(value);
                }
            }
            else
            {
                ret = new TuxRacerSettingsItem(key, new TuxRacerSettingsItemValue<T>(value));
                items.Add(key, ret);
            }
            return ret;
        }

        public TuxRacerSettingsItem get(string key)
        {
            TuxRacerSettingsItem ret = null;
            if (items.ContainsKey(key))
                ret = items[key];
            return ret;
        }

        private void parseConfigLine(string line)
        {
            string t = "", left, right = "";
            int i, len = line.Length;
            bool no_comment = true, in_string = false, is_string = false, break_loop = false;
            for (i = 0; i < len; i++)
            {
                if (line[i] == '#')
                {
                    if (i > 0)
                        t = line.Substring(0, i);
                    no_comment = false;
                    break;
                }
            }
            if (no_comment)
                t = line;
            t = t.Trim();
            len = t.Length;
            if (len > 0)
            {
                for (i = (len - 1); i >= 0; i--)
                {
                    switch (t[i])
                    {
                        case ' ':
                            if (!in_string)
                            {
                                if (i > 0)
                                {
                                    left = t.Substring(0, i).Trim();
                                    if ((len - i) > 0)
                                    {
                                        right = t.Substring(i, len - i).Trim();
                                        if (is_string)
                                        {
                                            if (right.Length >= 2)
                                            {
                                                if ((right[0] == '"') && (right[right.Length - 1] == '"'))
                                                    right = (right.Length > 2) ? right.Substring(1, right.Length - 2) : "";
                                            }
                                        }
                                    }
                                    if (is_string)
                                        add(left, right, false);
                                    else
                                    {
                                        if (right == "true")
                                            add(left, true, false);
                                        else if (right == "false")
                                            add(left, false, false);
                                        else
                                            add(left, Convert.ToInt32(right), false);
                                    }
                                }
                                break_loop = true;
                            }
                            break;
                        case '"':
                            is_string = true;
                            in_string = !in_string;
                            break;
                    }
                    if (break_loop)
                        break;
                }
            }
        }

        public void LoadDefaultValues()
        {
            TuxRacerSettingsItem item;
            items.Clear();

            item = add("set data_dir", ".");
            item.AddComment("data_dir");
            item.AddComment("");
            item.AddComment("The location of the Tux Racer data files");
            item.AddComment("");

            item = add("set fullscreen", true);
            item.AddComment("fullscreen");
            item.AddComment("");
            item.AddComment("If true then the game will run in full-screen mode.");
            item.AddComment("");

            item = add("set x_resolution", 640);
            item.AddComment("x_resolution");
            item.AddComment("");
            item.AddComment("The horizontal size of the Tux Racer window");
            item.AddComment("");

            item = add("set y_resolution", 480);
            item.AddComment("y_resolution");
            item.AddComment("");
            item.AddComment("The vertical size of the Tux Racer window");
            item.AddComment("");

            item = add("set bpp_mode", 0);
            item.AddComment("bpp_mode");
            item.AddComment("");
            item.AddComment("Controls how many bits per pixel are used in the game.");
            item.AddComment("Valid values are:");
            item.AddComment("");
            item.AddComment("0: Use current bpp setting of operating system");
            item.AddComment("1: 16 bpp");
            item.AddComment("2: 32 bpp");
            item.AddComment("Note that some cards (e.g., Voodoo1, Voodoo2, Voodoo3) only support");
            item.AddComment("16 bits per pixel.");
            item.AddComment("");

            item = add("set capture_mouse", false);
            item.AddComment("capture_mouse");
            item.AddComment("");
            item.AddComment("If true, then the mouse will not be able to leave the ");
            item.AddComment("Tux Racer window.");
            item.AddComment("If you lose keyboard focus while running Tux Racer, try setting");
            item.AddComment("this to true.");
            item.AddComment("");

            item = add("set force_window_position", false);
            item.AddComment("force_window_position");
            item.AddComment("");
            item.AddComment("If true, then the Tux Racer window will automatically be");
            item.AddComment("placed at (0,0)");
            item.AddComment("");

            item = add("set quit_key", "q escape");
            item.AddComment("quit_key");
            item.AddComment("");
            item.AddComment("Key binding for quitting a race");
            item.AddComment("");

            item = add("set turn_left_key", "j left");
            item.AddComment("turn_left_key");
            item.AddComment("");
            item.AddComment("Key binding for turning left");
            item.AddComment("");

            item = add("set turn_right_key", "l right");
            item.AddComment("turn_right_key");
            item.AddComment("");
            item.AddComment("Key binding for turning right");
            item.AddComment("");

            item = add("set trick_modifier_key", "d");
            item.AddComment("trick_modifier_key");
            item.AddComment("");
            item.AddComment("Key binding for doing tricks");
            item.AddComment("");

            item = add("set brake_key", "k space down");
            item.AddComment("brake_key");
            item.AddComment("");
            item.AddComment("Key binding for braking");
            item.AddComment("");

            item = add("set paddle_key", "i up");
            item.AddComment("paddle_key");
            item.AddComment("");
            item.AddComment("Key binding for paddling (on the ground) and flapping (in the air)");
            item.AddComment("");

            item = add("set jump_key", "e");
            item.AddComment("jump_key");
            item.AddComment("");
            item.AddComment("Key binding for jumping");
            item.AddComment("");

            item = add("set reset_key", "backspace");
            item.AddComment("reset_key");
            item.AddComment("");
            item.AddComment("Key binding for resetting the player position");
            item.AddComment("");

            item = add("set follow_view_key", "1");
            item.AddComment("follow_view_key");
            item.AddComment("");
            item.AddComment("Key binding for the \"Follow\" camera mode");
            item.AddComment("");

            item = add("set behind_view_key", "2");
            item.AddComment("behind_view_key");
            item.AddComment("");
            item.AddComment("Key binding for the \"Behind\" camera mode");
            item.AddComment("");

            item = add("set above_view_key", "3");
            item.AddComment("above_view_key");
            item.AddComment("");
            item.AddComment("Key binding for the \"Above\" camera mode");
            item.AddComment("");

            item = add("set view_mode", 1);
            item.AddComment("view_mode");
            item.AddComment("");
            item.AddComment("Default view mode. Possible values are");
            item.AddComment("");
            item.AddComment("  0: Behind");
            item.AddComment("  1: Follow");
            item.AddComment("  2: Above");
            item.AddComment("");
            
            item = add("set screenshot_key", "=");
            item.AddComment("screenshot_key");
            item.AddComment("");
            item.AddComment("Key binding for taking a screenshot");
            item.AddComment("");

            item = add("set pause_key", "p");
            item.AddComment("pause_key");
            item.AddComment("");
            item.AddComment("Key binding for pausing the game");
            item.AddComment("");

            item = add("set joystick_paddle_button", 0);
            item.AddComment("joystick_paddle_button");
            item.AddComment("");
            item.AddComment("Joystick button for paddling (numbering starts at 0).");
            item.AddComment("Set to -1 to disable.");
            item.AddComment("");

            item = add("set joystick_brake_button", 2);
            item.AddComment("joystick_brake_button");
            item.AddComment("");
            item.AddComment("Joystick button for braking (numbering starts at 0).");
            item.AddComment("Set to -1 to disable.");
            item.AddComment("");

            item = add("set joystick_jump_button", 3);
            item.AddComment("joystick_jump_button");
            item.AddComment("");
            item.AddComment("Joystick button for jumping (numbering starts at 0)");
            item.AddComment("");

            item = add("set joystick_trick_button", 1);
            item.AddComment("joystick_trick_button");
            item.AddComment("");
            item.AddComment("Joystick button for doing tricks (numbering starts at 0)");
            item.AddComment("");

            item = add("set joystick_continue_button", 0);
            item.AddComment("joystick_continue_button");
            item.AddComment("");
            item.AddComment("Joystick button for moving past intro, paused, and ");
            item.AddComment("game over screens (numbering starts at 0)");
            item.AddComment("");

            item = add("set joystick_x_axis", 0);
            item.AddComment("joystick_x_axis");
            item.AddComment("");
            item.AddComment("Joystick axis to use for turning (numbering starts at 0)");
            item.AddComment("");

            item = add("set joystick_y_axis", 1);
            item.AddComment("joystick_y_axis");
            item.AddComment("");
            item.AddComment("Joystick axis to use for paddling/braking (numbering starts at 0)");
            item.AddComment("");

            item = add("set no_audio", false);
            item.AddComment("no_audio");
            item.AddComment("");
            item.AddComment("If True, then audio in the game is completely disabled.");
            item.AddComment("");

            item = add("set sound_enabled", true);
            item.AddComment("sound_enabled");
            item.AddComment("");
            item.AddComment("Use this to turn sound effects on and off.");
            item.AddComment("");

            item = add("set music_enabled", true);
            item.AddComment("music_enabled");
            item.AddComment("");
            item.AddComment("Use this to turn music on and off.");
            item.AddComment("");

            item = add("set sound_volume", 127);
            item.AddComment("sound_volume");
            item.AddComment("");
            item.AddComment("This controls the sound volume (valid range is 0-127).");
            item.AddComment("");

            item = add("set music_volume", 64);
            item.AddComment("music_volume");
            item.AddComment("");
            item.AddComment("This controls the music volume (valid range is 0-127).");
            item.AddComment("");

            item = add("set audio_freq_mode", 1);
            item.AddComment("audio_freq_mode");
            item.AddComment("");
            item.AddComment("The controls the frequency of the audio.  Valid values are:");
            item.AddComment(" ");
            item.AddComment("  0: 11025 Hz");
            item.AddComment("  1: 22050 Hz");
            item.AddComment("  2: 44100 Hz");
            item.AddComment("");

            item = add("set audio_format_mode", 1);
            item.AddComment("audio_format_mode");
            item.AddComment("");
            item.AddComment("This controls the number of bits per sample for the audio.");
            item.AddComment("Valid values are:");
            item.AddComment("");
            item.AddComment("  0: 8 bits");
            item.AddComment("  1: 16 bits");
            item.AddComment("");

            item = add("set audio_stereo", true);
            item.AddComment("audio_stereo");
            item.AddComment("");
            item.AddComment("Audio will be played in stereo of true, and mono if false");
            item.AddComment("");

            item = add("set audio_buffer_size", 2048);
            item.AddComment("audio_buffer_size");
            item.AddComment("");
            item.AddComment("[EXPERT] Controls the size of the audio buffer.  ");
            item.AddComment("Increase the buffer size if you experience choppy audio");
            item.AddComment("(at the cost of greater audio latency)");
            item.AddComment("");

            item = add("set display_fps", false);
            item.AddComment("display_fps");
            item.AddComment("");
            item.AddComment("Set this to true to display the current framerate in Hz.");
            item.AddComment("");

            item = add("set course_detail_level", 75);
            item.AddComment("course_detail_level");
            item.AddComment("");
            item.AddComment("[EXPERT] This controls how accurately the course terrain is ");
            item.AddComment("rendered. A high value results in greater accuracy at the cost of ");
            item.AddComment("performance, and vice versa.  This value can be decreased and ");
            item.AddComment("increased in 10% increments at runtime using the F9 and F10 keys.");
            item.AddComment("To better see the effect, activate wireframe mode using the F11 ");
            item.AddComment("key (this is a toggle).");
            item.AddComment("");

            item = add("set forward_clip_distance", 75);
            item.AddComment("forward_clip_distance");
            item.AddComment("");
            item.AddComment("Controls how far ahead of the camera the course");
            item.AddComment("is rendered.  Larger values mean that more of the course is");
            item.AddComment("rendered, resulting in slower performance. Decreasing this ");
            item.AddComment("value is an effective way to improve framerates.");
            item.AddComment("");

            item = add("set backward_clip_distance", 10);
            item.AddComment("backward_clip_distance");
            item.AddComment("");
            item.AddComment("[EXPERT] Some objects aren't yet clipped to the view frustum, ");
            item.AddComment("so this value is used to control how far up the course these ");
            item.AddComment("objects are drawn.");
            item.AddComment("");

            item = add("set tree_detail_distance", 20);
            item.AddComment("tree_detail_distance");
            item.AddComment("");
            item.AddComment("[EXPERT] Controls the distance at which trees are drawn with ");
            item.AddComment("two rectangles instead of one.");
            item.AddComment("");

            item = add("set terrain_blending", true);
            item.AddComment("terrain_blending");
            item.AddComment("");
            item.AddComment("Controls the blending of the terrain textures.  Setting this");
            item.AddComment("to false will help improve performance.");
            item.AddComment("");

            item = add("set perfect_terrain_blending", false);
            item.AddComment("perfect_terrain_blending");
            item.AddComment("");
            item.AddComment("[EXPERT] If true, then terrain triangles with three different");
            item.AddComment("terrain types at the vertices will be blended correctly");
            item.AddComment("(instead of using a faster but imperfect approximation).");
            item.AddComment("");

            item = add("set terrain_envmap", true);
            item.AddComment("terrain_envmap");
            item.AddComment("");
            item.AddComment("If true, then the ice will be drawn with an \"environment map\",");
            item.AddComment("which gives the ice a shiny appearance.  Setting this to false");
            item.AddComment("will help improve performance.");
            item.AddComment("");

            item = add("set disable_fog", false);
            item.AddComment("disable_fog");
            item.AddComment("");
            item.AddComment("If true, then fog will be turned off.  Some Linux drivers for the");
            item.AddComment("ATI Rage128 seem to have a bug in their fog implementation which");
            item.AddComment("makes the screen nearly pure white when racing; if you experience");
            item.AddComment("this problem then set this variable to true.");
            item.AddComment("");

            item = add("set draw_tux_shadow", false);
            item.AddComment("draw_tux_shadow");
            item.AddComment("");
            item.AddComment("Set this to true to display Tux's shadow.  Note that this is a ");
            item.AddComment("hack and is quite expensive in terms of framerate.");
            item.AddComment("[EXPERT] This looks better if your card has a stencil buffer; ");
            item.AddComment("if compiling use the --enable-stencil-buffer configure option ");
            item.AddComment("to enable the use of the stencil buffer");
            item.AddComment("");

            item = add("set tux_sphere_divisions", 6);
            item.AddComment("tux_sphere_divisions");
            item.AddComment("");
            item.AddComment("[EXPERT] Higher values result in a more finely subdivided mesh ");
            item.AddComment("for Tux, and vice versa.  If you're experiencing low framerates,");
            item.AddComment("try lowering this value.");
            item.AddComment("");

            item = add("set tux_shadow_sphere_divisions", 3);
            item.AddComment("tux_shadow_sphere_divisions");
            item.AddComment("");
            item.AddComment("[EXPERT] The level of subdivision of Tux's shadow.");
            item.AddComment("");

            item = add("set draw_particles", true);
            item.AddComment("draw_particles");
            item.AddComment("");
            item.AddComment("Controls the drawing of snow particles that are kicked up as Tux");
            item.AddComment("turns and brakes.  Setting this to false should help improve ");
            item.AddComment("performance.");
            item.AddComment("");

            item = add("set track_marks", true);
            item.AddComment("track_marks");
            item.AddComment("");
            item.AddComment("If true, then the players will leave track marks in the snow.");
            item.AddComment("");

            item = add("set ui_snow", true);
            item.AddComment("ui_snow");
            item.AddComment("");
            item.AddComment("If true, then the ui screens will have falling snow.");
            item.AddComment("");

            item = add("set nice_fog", true);
            item.AddComment("nice_fog");
            item.AddComment("");
            item.AddComment("[EXPERT] If true, then the GL_NICEST hint will be used when");
            item.AddComment("rendering fog.  On some cards, setting this to false may improve");
            item.AddComment("performance.");
            item.AddComment("");

            item = add("set use_cva", true);
            item.AddComment("use_cva");
            item.AddComment("");
            item.AddComment("[EXPERT] If true, then compiled vertex arrays will be used when");
            item.AddComment("drawing the terrain.  Whether or not this helps performance");
            item.AddComment("is driver- and card-dependent.");
            item.AddComment("");

            item = add("set cva_hack", true);
            item.AddComment("cva_hack");
            item.AddComment("");
            item.AddComment("Some card/driver combinations render the terrrain incorrectly");
            item.AddComment("when using compiled vertex arrays.  This activates a hack ");
            item.AddComment("to work around that problem.");
            item.AddComment("");

            item = add("set use_sphere_display_list", true);
            item.AddComment("use_sphere_display_list");
            item.AddComment("");
            item.AddComment("[EXPERT]  Mesa 3.1 sometimes renders Tux strangely when display ");
            item.AddComment("lists are used.  Setting this to false should solve the problem ");
            item.AddComment("at the cost of a few Hz.");
            item.AddComment("");

            item = add("set do_intro_animation", true);
            item.AddComment("do_intro_animation");
            item.AddComment("");
            item.AddComment("If false, then the introductory animation sequence will be skipped.");
            item.AddComment("");

            item = add("set mipmap_type", 3);
            item.AddComment("mipmap_type");
            item.AddComment("");
            item.AddComment("[EXPERT] Allows you to control which type of texture");
            item.AddComment("interpolation/mipmapping is used when rendering textures.  The");
            item.AddComment("values correspond to the following OpenGL settings:");
            item.AddComment("");
            item.AddComment(" 0: GL_NEAREST");
            item.AddComment(" 1: GL_LINEAR");
            item.AddComment(" 2: GL_NEAREST_MIPMAP_NEAREST");
            item.AddComment(" 3: GL_LINEAR_MIPMAP_NEAREST");
            item.AddComment(" 4: GL_NEAREST_MIPMAP_LINEAR");
            item.AddComment(" 5: GL_LINEAR_MIPMAP_LINEAR");
            item.AddComment("");
            item.AddComment("On some cards, you may be able to improve performance by");
            item.AddComment("decreasing this number, at the cost of lower image quality.");
            item.AddComment("");

            item = add("set ode_solver", 1);
            item.AddComment("ode_solver");
            item.AddComment("");
            item.AddComment("Selects the ODE (ordinary differential equation) solver.  ");
            item.AddComment("Possible values are:");
            item.AddComment("");
            item.AddComment("  0: Modified Euler     (fastest but least accurate)");
            item.AddComment("  1: Runge-Kutta (2,3)");
            item.AddComment("  2: Runge-Kutta (4,5)  (slowest but most accurate)");
            item.AddComment("");

            item = add("set fov", 60);
            item.AddComment("fov");
            item.AddComment("");
            item.AddComment("[EXPERT] Sets the camera field-of-view");
            item.AddComment("");

            item = add("set debug", "");
            item.AddComment("debug");
            item.AddComment("");
            item.AddComment("[EXPERT] Controls the Tux Racer debugging modes");
            item.AddComment("");

            item = add("set warning_level", 100);
            item.AddComment("warning_level");
            item.AddComment("");
            item.AddComment("[EXPERT] Controls the Tux Racer warning messages");
            item.AddComment("");

            item = add("set write_diagnostic_log", false);
            item.AddComment("write_diagnostic_log");
            item.AddComment("");
            item.AddComment("If true, then a file called diagnostic_log.txt will be generated");
            item.AddComment("which you should attach to any bug reports you make.");
            item.AddComment("To generate the file, set this variable to \"true\", and");
            item.AddComment("then run the game so that you reproduce the bug, if possible.");
            item.AddComment("");
        }

        public void Load(string file_name)
        {
            LoadDefaultValues();
            if (File.Exists(file_name))
            {
                using (StreamReader tr = new StreamReader(file_name))
                {
                    while (!(tr.EndOfStream))
                        parseConfigLine(tr.ReadLine());
                    tr.Dispose();
                }
            }
            else
                Save(file_name);
        }

        public void Save(string file_name)
        {
            if (File.Exists(file_name))
                File.Delete(file_name);
            using (StreamWriter sw = new StreamWriter(file_name))
            {
                sw.WriteLine("# Tux Racer 0.61 configuration file");
                sw.WriteLine("#");
                foreach (KeyValuePair<string, TuxRacerSettingsItem> i in items)
                    sw.Write(i.Value.ToString());
                sw.Dispose();
            }
        }
    }
}
