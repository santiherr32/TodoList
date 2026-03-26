import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import {
  provideLucideIcons, LucidePencil, LucideTrash2, LucidePlus, LucideSun, LucideMoon, LucideMonitor,
  LucideCircleCheck, LucideCircle, LucideX, LucideCalendar
} from '@lucide/angular';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    provideLucideIcons(
      LucidePencil, LucideTrash2, LucidePlus, LucideSun, LucideMoon, LucideMonitor,
      LucideCircleCheck, LucideCircle, LucideX, LucideCalendar
    ),
  ]
};