shader_type canvas_item;

// Uniform variables
uniform vec4 color = vec4(1.0); // source color
uniform float width = 1.0; // Range: 0 to 10
uniform int pattern = 0; // Patterns: 0 = diamond, 1 = circle, 2 = square
uniform bool inside = false;
uniform bool add_margins = true; // Only useful when inside is false
uniform vec2 number_of_images = vec2(1.0); // Number of horizontal and vertical images in the sprite sheet

void vertex() {
	if (add_margins) {
		// Adjust the vertex position to add margins
		VERTEX += sign(VERTEX) * width; // Replace `sign(VERTEX)` by `sign(VERTEX * 2.0 - 1.0)` if not centered
	}
}

bool hasContraryNeighbour(vec2 uv, vec2 texture_pixel_size, vec2 image_top_left, vec2 image_bottom_right, sampler2D texture) {
	for (float i = -ceil(width); i <= ceil(width); i++) {
		float x = abs(i) > width ? width * sign(i) : i;
		float offset;
		
		// Calculate the offset based on the selected pattern
		if (pattern == 0) { // Diamond
			offset = width - abs(x);
		} else if (pattern == 1) { // Circle
			offset = floor(sqrt(pow(width + 0.5, 2) - x * x));
		} else if (pattern == 2) { // Square
			offset = width;
		}
		
		for (float j = -ceil(offset); j <= ceil(offset); j++) {
			float y = abs(j) > offset ? offset * sign(j) : j;
			vec2 xy = uv + texture_pixel_size * vec2(x, y);
			
			// Check for contrary neighbour pixels
			if ((xy != clamp(xy, image_top_left, image_bottom_right) || texture(texture, xy).a <= 0.0) == inside) {
				return true;
			}
		}
	}
	
	return false;
}

void fragment() {
	vec2 uv = UV;
	vec2 image_top_left = floor(uv * number_of_images) / number_of_images;
	vec2 image_bottom_right = image_top_left + vec2(1.0) / number_of_images;

	if (add_margins) {
		vec2 texture_pixel_size = vec2(1.0) / (vec2(1.0) / TEXTURE_PIXEL_SIZE + vec2(width * 2.0) * number_of_images);
		
		// Adjust UV coordinates based on the added margins
		uv = (uv - texture_pixel_size * width - image_top_left) * TEXTURE_PIXEL_SIZE / texture_pixel_size + image_top_left;
		
		// Handle margin-based transparency
		if (uv != clamp(uv, image_top_left, image_bottom_right)) {
			COLOR.a = 0.0;
		} else {
			COLOR = texture(TEXTURE, uv);
		}
	} else {
		COLOR = texture(TEXTURE, uv);
	}
	
	// Apply color mixing based on the inside flag and the presence of contrary neighbours
	if ((COLOR.a > 0.0) == inside && hasContraryNeighbour(uv, TEXTURE_PIXEL_SIZE, image_top_left, image_bottom_right, TEXTURE)) {
		COLOR.rgb = inside ? mix(COLOR.rgb, color.rgb, color.a) : color.rgb;
		COLOR.a += (1.0 - COLOR.a) * color.a;
	}
}
