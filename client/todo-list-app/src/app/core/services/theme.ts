import { Injectable, signal, effect } from '@angular/core';

export type Theme = 'light' | 'dark' | 'system';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  theme = signal<Theme>(this.getSavedTheme());

  constructor() {
    effect(() => {
      this.applyTheme(this.theme());
      localStorage.setItem('theme', this.theme());
    });
  }

  setTheme(theme: Theme): void {
    this.theme.set(theme);
  }

  private getSavedTheme(): Theme {
    return (localStorage.getItem('theme') as Theme) ?? 'system';
  }

  private applyTheme(theme: Theme): void {
    const root = document.documentElement;
    if (theme === 'dark') {
      root.classList.add('dark');
    } else if (theme === 'light') {
      root.classList.remove('dark');
    } else {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      prefersDark ? root.classList.add('dark') : root.classList.remove('dark');
    }
  }
}