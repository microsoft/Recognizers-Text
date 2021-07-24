// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

export class MetaData {
    private isHoliday: boolean = false;

    get IsHoliday(): boolean {
        return this.isHoliday;
    }
    set IsHoliday(isHoliday: boolean) {
        this.isHoliday = isHoliday;
    }

    private hasMod: boolean = false;

    get HasMod(): boolean {
        return this.hasMod;
    }
    set HasMod(hasMod: boolean) {
        this.hasMod = hasMod;
    }

    private isDurationWithAgoAndLater: boolean = false;

    get IsDurationWithAgoAndLater(): boolean {
        return this.isDurationWithAgoAndLater;
    }
    set IsDurationWithAgoAndLater(isDurationWithAgoAndLater: boolean) {
        this.isDurationWithAgoAndLater = isDurationWithAgoAndLater;
    }
}
