#pragma once

namespace AntiGrainRT
{
  public ref class RenderingBuffer sealed
  {
    public:
    RenderingBuffer(unsigned int width, unsigned int height);
    void SetPixel(unsigned int x, unsigned int y, Windows::UI::Color color);
    Windows::Foundation::IAsyncAction^ SavePpm(Windows::Storage::IStorageFile^ file);
    property unsigned int PixelWidth { unsigned int get() { return m_width; } }
    property unsigned int PixelHeight { unsigned int get() { return m_height; } }

  private:
    agg::rendering_buffer m_rbuf;
    Platform::Array<uint8>^ m_buffer;
    unsigned int m_width;
    unsigned int m_height;
  };
}