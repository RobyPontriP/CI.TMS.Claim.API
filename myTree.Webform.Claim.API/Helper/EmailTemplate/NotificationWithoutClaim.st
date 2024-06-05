<style type="text/css">
  <!--    .table {    font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;    border-collapse: collapse;    width: 100%;    font-size: 14px;    }      .table td, table th {    border: 1px solid #ddd;    padding: 8px;    }      .zebra {background-color: #E1F7F0;}      .table th {    padding-top: 12px;    padding-bottom: 12px;    text-align: left;    background-color: #00a46c;    color: white;    }      .table-header {    font-weight: bold;    }    .m-link-white{    color:#FFFFFF;    }-->
  p, ul {
  font-family: 'Verdana';
  font-size: 12px;
  }
  .table {
  font-family: 'Verdana';
  font-size: 12px;
  border-collapse: collapse;
  width: 100%;
  vertical-align: top;
  }
  table {
  font-family: 'Verdana';
  font-size: 12px;
  border-collapse: collapse;
  width: 100%;
  }
  .table td, table th {
  border: 1px solid #ddd;
  padding: 5px;
  }
  .zebra {
  background-color: #E1F7F0;
  }
  .table th {
  padding-top: 12px;
  padding-bottom: 12px;
  background-color: #00a46c;
  color: white;
  text-align: center;
  }
  .table-header {
  font-weight: bold;
  }
  .m-link-white{
  color:#FFFFFF;
  }
  .email-footer {
  color:#f1be24;
  text-decoration:none;
  }
  .tableheader {
  font-family: Verdana, Geneva, sans-serif;
  font-size: 14px;
  background-color: #9BBB59;
  font-weight: bold;
  color: #FFF;   padding:5px;
  }
  .header {
  font-family: Verdana, Geneva, sans-serif;
  font-size: 18px;
  font-weight: bold;
  color: black;
  padding:5px;
  text-align:center;
  }
  .bookingtitle {
  font-family: Verdana, Geneva, sans-serif;
  font-size: 15px;
  color: midnightblue;
  padding:5px;
  text-align:center;
  }
  .header2 {
  padding-top: 12px;
  padding-bottom: 12px;
  background-color: #00a46c;
  color: white;
  text-align: left;
  border-bottom: #e0e0d1 1px solid;
</style>

$if(TEST_MODE)$
$TO_CC$
$endif$


<p>Dear all,</p>
<p>
  This is to inform you that the following TA has nothing to claim.
</p>


$if(TRAVELER_TYPE)$
<table border="1" cellspacing="0" cellpadding="4" class="table">
  <tr>
    <td  width="20%" class="header2">
      <b>
        Staff responsible
      </b>&nbsp;
    </td>
    <td  width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_NAME$
      </b>
    </td>
  </tr>
  <tr>
    <td  width="20%" class="header2">
      <b>
        Duty post
      </b>&nbsp;
    </td>
    <td  width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_DUTY_POST$
      </b>
    </td>
  </tr>
  <tr>
    <td  width="20%" class="header2">
      <b>
        Working location
      </b>&nbsp;
    </td>
    <td  width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_WORKING_LOCATION$
      </b>
    </td>
  </tr>
</table>

$endif$
$if(!TRAVELER_TYPE)$
<table border="1" cellspacing="0" cellpadding="4" class="table-title">
  <tr>
    <td width="20%" class="header2">
      <b>
        Traveler
      </b>&nbsp;
    </td>
    <td width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_NAME$
      </b>
    </td>
  </tr>
  <tr>
    <td width="20%" class="header2">
      <b>
        Gender
      </b>&nbsp;
    </td>
    <td width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_GENDER$
      </b>
    </td>
  </tr>
  <tr>
    <td width="20%" class="header2">
      <b>
        Duty post
      </b>&nbsp;
    </td>
    <td width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_DUTY_POST$
      </b>
    </td>
  </tr>
  <tr>
    <td width="20%" class="header2">
      <b>
        Working location
      </b>&nbsp;
    </td>
    <td width="80%" style="padding-top: 0px">
      <b>
        $TRAVELER_WORKING_LOCATION$
      </b>
    </td>
  </tr>

</table>
$endif$
</br>

<!--DESTINATION-->
<p>
  <table style="width:100%">
    <tr>
      <td width="20%"  valign="top" style="font-size:14px">
        <strong> Destination(s)</strong>
      </td>
    </tr>
  </table>
  $destinations$
</p>


<!--ITINERARY-->
$if(TicketRequiredApproval)$
<p>
  <table style="width:100%">
    <tr>
      <td width="20%"  valign="top" style="font-size:14px">
        <strong>Itinerary</strong>
      </td>
    </tr>
  </table>
  $itinerary$
</p>
$endif$



<p>
  <table border="0" cellspacing="0" cellpadding="5" widtd="100%" class="table">
    <tr>
      <td class="header2" width="20%" style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Actual airfare</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$ActualAirfare$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Air ticket is required?</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$IsTicketRequired$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Hotel voucher required?</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$IsHotelRequired$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Travel insurance required?</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$IsTravelInsuranceRequired$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Preferences</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$Preferences$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Additional information</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$AdditionalInformation$</p>
      </td>
    </tr>
    $if(TicketRequiredApproval)$
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Flight tickets</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$FlightTicket$</p>
      </td>
    </tr>
    <tr>
      <td class="header2" width="20%" style="border-bottom: #e0e0d1 solid 1px" >
        <p>Type of booking</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$TypeOfBooking$</p>
      </td>
    </tr>
    $endif$
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Travel advance required</p>
      </td>
      $if(IsAdvanceRequired)$
      $if(IsAllAmount)$
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$AllAmountAdvance$</p>
      </td>
      $endif$
      $if(!IsAllAmount)$
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$TextAmountAdvance$</p>
      </td>
      $endif$
      $endif$
      $if(!IsAdvanceRequired)$
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>No advance required</p>
      </td>
      $endif$
    </tr>
    $if(IsAdvanceRequired)$
    <tr>
      <td class="header2" width="20%"  style="border-bottom: #e0e0d1 solid 1px;" >
        <p>Travel advance required date</p>
      </td>
      <td width="80%" style="border-bottom: #e0e0d1 solid 1px">
        <p>$AdvanceDate$</p>
      </td>
    </tr>
    $endif$
  </table>
</p>

$if(hasCostCenter)$
<p>
  <table style="width:100%">
    <tr>
      <td width="20%"  valign="top" style="font-size:14px">
        <strong>Charge code(s) information</strong>
      </td>
    </tr>
  </table>
  $datachargecodes$
</p>
$endif$

<!--$if(!is_personal)$
<p>
  Please provide the tickets
</p>
<p>
  $endif$-->


<p>Thank you.</p>
<br></br>
<!-- FOOTER! -->
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="x_min-width" style="table-layout:fixed">
  <tbody>
    <tr>
      <td bgcolor="#111010" align="center" style="padding:25px 15px 25px 15px">
        <table border="0" width="100%" cellspacing="0" cellpadding="0" align="center">
          <tbody>
            <tr>
              <td align="center" bgcolor="#111010" style="padding:0; border:0px">
                <table border="0" width="100%" cellspacing="0" cellpadding="0" align="center">
                  <tbody>
                    <tr style="">
                      <td align="center" bgcolor="#111010" style="font-size:14px; color:#dedede; line-height:23px; border:0px">
                        Please do not reply to this message. Replies to this message are Preferencesd to an unmonitored mailbox. If you have any questions, please do not hesitate to email $TEAMFINANCE$
                      </td>
                    </tr>
                  </tbody>
                </table>
              </td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
  </tbody>
</table>