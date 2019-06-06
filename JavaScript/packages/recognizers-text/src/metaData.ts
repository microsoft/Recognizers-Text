export class MetaData {
    private isHoliday: boolean = false;

    get IsHoliday(): boolean {
        return this.isHoliday;
    }
    set IsHoliday(isHoliday: boolean) {
        this.isHoliday = isHoliday
    }
}
