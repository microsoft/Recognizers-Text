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

    private isDurationWithBeforeAndAfter: boolean = false;

    get IsDurationWithBeforeAndAfter(): boolean {
        return this.isDurationWithBeforeAndAfter;
    }
    set IsDurationWithBeforeAndAfter(isDurationWithBeforeAndAfter: boolean) {
        this.isDurationWithBeforeAndAfter = isDurationWithBeforeAndAfter;
    }
}
