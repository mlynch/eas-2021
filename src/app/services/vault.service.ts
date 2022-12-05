import { Injectable } from '@angular/core';
import { Capacitor } from '@capacitor/core';
import {
    BiometricSecurityStrength,
    BrowserVault, Device, DeviceSecurityType,
    IdentityVaultConfig, Vault, VaultError, VaultErrorCodes, VaultType
} from '@ionic-enterprise/identity-vault';
import { AuthenticationService } from './authentication.service';
import { RouteService } from './route.service';

@Injectable({
    providedIn: 'root'
})
export class VaultService {

    constructor(private routeService: RouteService) {
    }

    public async init() {

    }

    public async clear() {
    }

    public async set(key: string, value: any) {
    }

    public async get(key: string): Promise<any> {
        return { key: 'foo' };
    }

}
