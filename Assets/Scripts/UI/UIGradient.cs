using UnityEngine;

public static class UIGradient {
    public static Texture2D CreateDividerTexture() {
        int width = 256;
        int height = 16;

        Texture2D texture = new Texture2D(
            width,
            height,
            TextureFormat.RGBA32,
            false
        );

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;

        Color[] pixels = new Color[width * height];

        for (int x = 0; x < width; x++) {
            float u = (float)x / (width - 1);

            // Fade horizontally
            float horizontal = Mathf.Sin(u * Mathf.PI);
            horizontal = Mathf.Pow(horizontal, 0.7f);

            // Thickness increases towards the center
            float thickness = Mathf.Lerp(
                0.15f,
                0.5f,
                horizontal
            );

            for (int y = 0; y < height; y++) {
                float v = (float)y / (height - 1);

                // Fade vertically to create thin edges
                float verticalDistance = Mathf.Abs(v - 0.5f);

                float vertical = 1f - Mathf.SmoothStep(
                    thickness * 0.5f - 0.08f,
                    thickness * 0.5f,
                    verticalDistance
                );

                float alpha = horizontal * vertical;

                pixels[y * width + x] = new Color(
                    1f, 1f, 1f, alpha
                );
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}