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
	padding: 8px;
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
    .table-td{
        width: 130px;
        background-color: #00a46c;
        color: white;
    }
</style>
$if(TEST_MODE)$
$TO_CC$
$endif$

<p>Dear all,</p>
<p>
	This is to inform you that the following claim has been approved by $EMPLOYEE$.
</p>

$if(TRAVELER_TYPE)$
<table border="1" cellspacing="0" cellpadding="4" class="table-title">
	<tr>
	  	<td class="table-td">
			<b>
				Staff responsible
			</b>&nbsp;
		</td>
		<td style="padding-top: 0px">
			<b>
				$TRAVELER_NAME$
			</b>
		</td>
	</tr>
	<tr>
	  	<td class="table-td">
			<b>
				Duty post
			</b>&nbsp;
		</td>
		<td style="padding-top: 0px">
			<b>
				$TRAVELER_DUTY_POST$
			</b>
		</td>
	</tr>
	<tr>
	  	<td class="table-td">
			<b>
				Working location
			</b>&nbsp;
		</td>
		<td style="padding-top: 0px">
			<b>
				$TRAVELER_WORKING_LOCATION$
			</b>
		</td>
	</tr>
    <tr>
        <td class="table-td">
            <b>
                TA code
            </b>&nbsp;
        </td>
        <td style="padding-top: 0px">
            <b>
                <a href="$TA_PROFILE_URL$" target="_blank">
                    $TA_CODE$
                </a>
            </b>
        </td>
    </tr>
    <tr>
        <td class="table-td">
            <b>
               TEC code
            </b>&nbsp;
        </td>
        <td style="padding-top: 0px">
            <b>
                <a href="$CLAIM_PROFILE_URL$" target="_blank">
                    $TEC_CODE$
                </a>
            </b>
        </td>
    </tr>
    <tr>
        <td class="table-td">
            <b>
                Finance comment
            </b>&nbsp;
        </td>
        <td style="padding-top: 0px">
            <b>
                $COMMENT$
            </b>
        </td>
    </tr>
</table>
$endif$
$if(!TRAVELER_TYPE)$
	<table border="1" cellspacing="0" cellpadding="4" class="table-title">
		<tr>
	  	<td class="table-td">
			<b>
				Traveler
			</b>&nbsp;
		</td>
		<td style="padding-top: 0px">
			<b>
				$TRAVELER_NAME$
			</b>
		</td>
		</tr>
		<tr>
			<td class="table-td">
				<b>
					Gender
				</b>&nbsp;
			</td>
			<td style="padding-top: 0px">
				<b>
					$TRAVELER_GENDER$
				</b>
			</td>
		</tr>
		<tr>
	  	<td class="table-td">
			<b>
				Duty post
			</b>&nbsp;
		</td>
		<td style="padding-top: 0px">
			<b>
				$TRAVELER_DUTY_POST$
			</b>
		</td>
		</tr>
		<tr>
			<td class="table-td">
				<b>
					Working location
				</b>&nbsp;
			</td>
			<td style="padding-top: 0px">
				<b>
					$TRAVELER_WORKING_LOCATION$
				</b>
			</td>
		</tr>
        <tr>
            <td class="table-td">
                <b>
                    TA code
                </b>&nbsp;
            </td>
            <td style="padding-top: 0px">
                <b>
                    <a href="$TA_PROFILE_URL$" target="_blank">
                        $TA_CODE$
                    </a>
                </b>
            </td>
        </tr>
        <tr>
            <td class="table-td">
                <b>
                    TEC code
                </b>&nbsp;
            </td>
            <td style="padding-top: 0px">
                <b>
                    <a href="$CLAIM_PROFILE_URL$" target="_blank">
                        $TEC_CODE$
                    </a>
                </b>
            </td>
        </tr>
        <tr>
            <td class="table-td">
                <b>
                    Finance comment
                </b>&nbsp;
            </td>
            <td style="padding-top: 0px">
                <b>
                    $COMMENT$
                </b>
            </td>
        </tr>
	</table>
$endif$

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
<p>
	Please contact the Finance for more information.
</p>
<br />
<p>
	Thank you for your attention.
</p>


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
												Please do not reply to this message. Replies to this message are routed to an unmonitored mailbox. If you have any questions, please do not hesitate to email $TEAMFINANCE$
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