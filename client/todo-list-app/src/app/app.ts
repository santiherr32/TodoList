import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeService, Theme } from './core/services/theme';
import { LucideDynamicIcon } from '@lucide/angular';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LucideDynamicIcon],
  templateUrl: './app.html',
})

export class App {
  readonly themeService = inject(ThemeService);

  themeOptions: { value: Theme; icon: string; label: string }[] = [
    { value: 'light', icon: 'sun', label: 'Claro' },
    { value: 'dark', icon: 'moon', label: 'Oscuro' },
    { value: 'system', icon: 'monitor', label: 'Sistema' },
  ];

  setTheme(theme: Theme): void {
    this.themeService.setTheme(theme);
  }
}