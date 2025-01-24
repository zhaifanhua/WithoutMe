export interface Song {
  name: string;
  artist: string;
  musicUrl: string;
  lyricsUrl: string;
  coverUrl: string;
  duration: number;
}

export const playList: Song[] = [
  {
    name: "我们都被忘了",
    artist: "谢安琪",
    musicUrl: "/audios/musics/1.mp3",
    lyricsUrl: "/audios/lyrics/1.lrc",
    coverUrl: "/audios/covers/1.jpg",
    duration: 2546,
  },
  {
    name: "壁上观",
    artist: "邓寓君(等什么君)",
    musicUrl: "/audios/musics/2.m4a",
    lyricsUrl: "/audios/lyrics/2.lrc",
    coverUrl: "/audios/covers/2.jpg",
    duration: 2546,
  },
];
