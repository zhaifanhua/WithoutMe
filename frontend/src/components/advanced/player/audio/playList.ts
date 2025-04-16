export interface Song {
  name: string;
  artist: string;
  musicUrl: string;
  lyricsUrl: string;
  coverUrl: string;
}

export const playList: Song[] = [
  {
    name: "晚风心里吹",
    artist: "阿梨粤",
    musicUrl: "/audios/阿梨粤 - 晚风心里吹.flac",
    lyricsUrl: "/audios/阿梨粤 - 晚风心里吹.lrc",
    coverUrl: "/audios/阿梨粤 - 晚风心里吹.jpg",
  },
];
