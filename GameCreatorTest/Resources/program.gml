#define main
{
	click = "
		if (t == 1)
		{
			if (window_count() > 1)
				window_close(window_index);
			else
			{
				show_message('Ha ha! the game ends!');
				game_end()
			}
		}
		else
		{
			t-=1
			show_message('You have '+string(t)+' tries left.');
		}";
	w = window_create("My Window");
	window_set_color(w, choose(c_red, c_green, c_yellow, c_blue));
	window_set_create(w, "t = 3");
	window_set_click(w, click);
	window_add_button(w, 16, 16, 80, 25, "Click me!");
	window_show(w);
}
