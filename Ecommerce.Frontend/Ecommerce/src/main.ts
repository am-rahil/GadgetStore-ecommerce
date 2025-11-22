import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';
import { ModuleRegistry, AllCommunityModule } from 'ag-grid-community';
ModuleRegistry.registerModules([AllCommunityModule]);


platformBrowser().bootstrapModule(AppModule, {
  
})
  .catch(err => console.error(err));
